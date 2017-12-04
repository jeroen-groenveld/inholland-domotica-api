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
    public class RefreshTokenSubmit
    {
        [Required]
        [MaxLength(88)]
        public string token { get; set; }
    }


    [Route(Config.App.API_ROOT_PATH + "/auth")]
    public class TokenController : ApiController
    {
        //Constructor
        public TokenController(DatabaseContext db) : base(db) { }

        [HttpPost("authorize")]
        public IActionResult Authorize([FromBody] UserLogin userLogin)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            User user = UserController.Authenticate(userLogin);
            if(user == null)
            {
                return BadRequest("Incorrect credentials.");
            }

            return this.GenerateTokens(user);
        }

        [HttpPost("token/refresh")]
        public IActionResult RefreshAccessToken([FromBody] RefreshTokenSubmit refreshToken)
        {
            //Check post data.
            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            //Check token length.
            if (refreshToken.token.Length != 88)
            {
                return BadRequest("Token length is invalid.");
            }

            //Check if the refresh token exists in the database.
            AccessToken accessToken = this.db.AccessTokens.Where(x => x.token == refreshToken.token && x.refresh_token != null).Include(x => x.refresh_token).Include(x => x.user).FirstOrDefault();
            if (accessToken == null)
            {
                return BadRequest("Token not found.");
            }

            //Remove old tokens for this user.
            //Retrieve all refresh tokens that belong to this user.
            List<RefreshToken> oldRefreshTokens = this.db.RefreshTokens.Where(x => x.access_token.user_id == accessToken.user_id).Include(x => x.access_token).ToList();
            //Delete them from the database.
            this.db.RemoveRange(oldRefreshTokens);

            //Retrieve all access tokens that belong to this user.
            List<AccessToken> oldAccessTokens = this.db.AccessTokens.Where(x => x.user_id == accessToken.user_id).ToList();
            //Delete them from the database.
            this.db.RemoveRange(oldAccessTokens);

            //Save the changes.
            this.db.SaveChanges();


            //Check if the refresh token has expired.
            if (accessToken.expires_at < DateTime.Now)
            {
                return BadRequest("Token expired.");
            }

            //Generate new tokens.
            return this.GenerateTokens(accessToken.user);
        }

        private IActionResult GenerateTokens(User user)
        {
            AccessToken accessToken = this.GenerateToken(user);
            AccessToken refreshAccessToken = this.GenerateToken(user, false);

            //Create refresh token model
            RefreshToken refreshToken = new RefreshToken()
            {
                access_token = refreshAccessToken,
            };

            //Save access & refresh token to the database.
            this.db.Add(accessToken);
            this.db.Add(refreshAccessToken);

            //Save the refresh token to the database that links to the refreshAccessToken(Which is of type AccessToken)
            this.db.Add(refreshToken);

            //Save changes to the database.
            this.db.SaveChanges();

            //Return an array that contains the tokens and their expire date.
            object result = new
            {
                access_token = accessToken.token,
                access_token_expire = accessToken.expires_at.ToString("MM-dd-yyyy HH:mm:ss"),
                refresh_token = refreshAccessToken.token,
                refresh_token_expire = refreshAccessToken.expires_at.ToString("MM-dd-yyyy HH:mm:ss"),
            };

            return Ok(result);
        }

        private AccessToken GenerateToken(User user, bool is_access_token = true)
        {
            //Generate a rand unique string.
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