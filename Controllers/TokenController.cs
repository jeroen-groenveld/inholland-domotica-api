using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Domotica_API.Models;
using Domotica_API.Models.TokenAuth;

namespace Domotica_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/auth")]
    public class TokenController : ApiController
    {
        //Constructor
        public TokenController(DatabaseContext db) : base(db) { }

        //End-Point for gaining a new Access Token and Refresh Token. Validation is done by the given User Credentials.
        [HttpPost("authorize")]
        public IActionResult Authorize([FromBody] Validators.User.UserLogin RequestData)
        {
            //Check post data.
            if(ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            //Get autheneticated user.
            User user = UserController.Authenticate(RequestData);
            if(user == null)
            {
                return BadRequest("Incorrect credentials.");
            }

            //Before generating new tokens for this user, delete all existing tokens.
            this.DeleteAccessTokensForUser(user);

            //Generate tokens for this user.
            return this.GenerateTokens(user);
        }

        //This End-Point is used for gaining a new Access Token and Refresh Token. Validation is done by the given Refresh Token.
        [HttpPost("token/refresh")]
        public IActionResult RefreshAccessToken([FromBody] Validators.RefreshToken RequestData)
        {
            //Check post data.
            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            //Check token length.
            if (RequestData.token.Length != 88)
            {
                return BadRequest("Token length is invalid.");
            }

            //Check if the given RefreshToken exists in the database.
            AccessToken RefreshToken = this.db.AccessTokens.Where(x => x.token == RequestData.token && x.refresh_token != null).Include(x => x.refresh_token).Include(x => x.user).FirstOrDefault();
            if (RefreshToken == null)
            {
                return BadRequest("Token not found.");
            }

            //Check if the refresh token has expired.
            if (RefreshToken.expires_at < DateTime.Now)
            {
                //Delete the expired tokens.
                this.DeleteExpiredAccessTokensForUser(RefreshToken.user);

                return BadRequest("Token expired.");
            }

            //Before generating new tokens for this user, delete all existing tokens.
            this.DeleteAccessTokensForUser(RefreshToken.user);

            //Generate new tokens.
            return this.GenerateTokens(RefreshToken.user);
        }

        private void DeleteExpiredAccessTokensForUser(User user)
        {
            //Retrieve all refresh tokens that belong to this user and are expired.
            List<RefreshToken> RefreshTokens = this.db.RefreshTokens.Where(x => x.access_token.user_id == user.id && x.access_token.expires_at < DateTime.Now).Include(x => x.access_token).ToList();

            //Retrieve all access tokens that belong to this user and are expired.
            List<AccessToken> AccessTokens = this.db.AccessTokens.Where(x => x.user_id == user.id && x.expires_at < DateTime.Now).ToList();

            //Delete them from the database.
            this.DeleteTokens(RefreshTokens, AccessTokens);
        }

        private void DeleteAccessTokensForUser(User user)
        {
            //Retrieve all refresh tokens that belong to this user.
            List<RefreshToken> RefreshTokens = this.db.RefreshTokens.Where(x => x.access_token.user_id == user.id).Include(x => x.access_token).ToList();

            //Retrieve all access tokens that belong to this user.
            List<AccessToken> AccessTokens = this.db.AccessTokens.Where(x => x.user_id == user.id).ToList();

            //Delete them from the database.
            this.DeleteTokens(RefreshTokens, AccessTokens);
        }

        private void DeleteTokens(List<RefreshToken> RefreshTokens, List<AccessToken> AccessTokens)
        {
            bool no_tokens = true;
            if(RefreshTokens.Count > 0)
            {
                this.db.RemoveRange(RefreshTokens);
                no_tokens = false;
            }
            if(AccessTokens.Count > 0)
            {
                this.db.RemoveRange(AccessTokens);
                no_tokens = false;
            }

            if(no_tokens == false)
            {
                this.db.SaveChanges();
            }
        }

        private IActionResult GenerateTokens(User user)
        {
            AccessToken AccessToken = this.GenerateToken(user);
            AccessToken RefreshAccessToken = this.GenerateToken(user, false);

            //Create refresh token model
            RefreshToken RefreshToken = new RefreshToken()
            {
                access_token = RefreshAccessToken,
            };

            //Save access & refresh token to the database.
            this.db.Add(AccessToken);
            this.db.Add(RefreshAccessToken);

            //Save the refresh token to the database that links to the refreshAccessToken(Which is of type AccessToken)
            this.db.Add(RefreshToken);

            //Save changes to the database.
            this.db.SaveChanges();

            //Return an array that contains the tokens and their expire date.
            object result = new
            {
                access_token = AccessToken.token,
                access_token_expire = AccessToken.expires_at.ToString("MM-dd-yyyy HH:mm:ss"),
                refresh_token = RefreshAccessToken.token,
                refresh_token_expire = RefreshAccessToken.expires_at.ToString("MM-dd-yyyy HH:mm:ss"),
            };

            return Ok(result);
        }

        private AccessToken GenerateToken(User user, bool is_access_token = true)
        {
            //Generate a random unique string. If users get the same token weird shit will happen.
            Random random = new Random();
            string uid = random.Next(0, 1000000) + DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss.ffffff") + user.email;
            uid += (is_access_token) ? "" : "_refresh";

            //Hash the string to a 64 byte sha512 hash.
            string str_token;
            using (SHA512 sha = new SHA512Managed())
            {
                str_token = Convert.ToBase64String(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(uid)));
            }

            //Create access token instance.
            AccessToken token = new AccessToken()
            {
                token = str_token,
                user_id = user.id,
                expires_at = DateTime.Now.AddMinutes((is_access_token) ? Config.App.API_ACCESS_TOKEN_EXPIRE : Config.App.API_REFRESH_TOKEN_EXPIRE)
            };

            return token;
        }
    }
}