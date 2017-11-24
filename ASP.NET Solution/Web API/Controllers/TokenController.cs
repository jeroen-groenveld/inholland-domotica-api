using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Models.TokenAuth;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Web_API.Controllers
{

    [Route(Config.Api.API_ROOT_PATH + "/auth")]
    public class TokenController : ApiController
    {
        //APP Key.
        byte[] SALT = UTF8Encoding.UTF8.GetBytes("Sosgq78hIbV5hd8KvD2Z56c2hPmbrcsTBpGcMx8q0Yu9s8qSVT3ecQMTyP6W1bHs");
        int ACCESS_TOKEN_EXPIRE = 10;
        int REFRESH_TOKEN_EXPIRE = 30;

        //Constructor
        public TokenController(DatabaseContext db) : base(db) { }

        [HttpPost("authorize")]
        public ApiResult Authorize(MUserLogin userLogin)
        {
            try
            {
                //Check if the model is valid.
                if (ModelState.IsValid)
                {
                    User user;
                    ApiResult error_result = this.AuthenticateUser(userLogin, out user);
                    if(user == null)
                    {
                        return error_result;
                    }

                    return this.GenerateTokens(user);
                }
            } catch { }

            return new ApiResult("Not Authorized.");
        }

        [HttpPost("token/refresh")]
        public ApiResult RefreshToken(MRefreshToken refreshToken)
        {
            //try
            //{
                //Check if the model is valid.
                if (ModelState.IsValid)
                {
                    return this.RefreshAccessToken(refreshToken);
                }
            //}
            //catch { }

            return new ApiResult("Invalid request", true);
        }

        [HttpPost("register")]
        public ApiResult Register(MUserRegister userRegister)
        {
            try
            {
                //Check if the model is valid.
                if (ModelState.IsValid)
                {
                    return this.RegisterUser(userRegister);
                }
            }
            catch { }

            return new ApiResult("Wrong credentials.");
        }

        private ApiResult AuthenticateUser(MUserLogin userLogin, out User _user)
        {
            _user = null;
            User user = this._db.Users.Where(x => x.email == userLogin.email).FirstOrDefault();

            //Check if user exists
            if (user == null)
            {
                return new ApiResult("User does not exist.", true);
            }

            //Get sha 512 hash of the password stored in the database.
            byte[] db_pass_hash_with_salt = Convert.FromBase64String(user.password);

            //Hash the given password and check it against database password.
            using (SHA512 sha = new SHA512Managed())
            {
                byte[] pass = UTF8Encoding.UTF8.GetBytes(userLogin.password);
                byte[] pass_with_salt = pass.Concat(this.SALT).ToArray();

                byte[] pass_hash_with_salt = sha.ComputeHash(pass_with_salt);
                if(db_pass_hash_with_salt.SequenceEqual(pass_hash_with_salt))
                {
                    _user = user;
                    return null;
                }
            }

            return new ApiResult("Wrong password.", true);
        }

        private ApiResult RegisterUser(MUserRegister userRegister)
        {
            if(this._db.Users.Where(x => x.email == userRegister.email).FirstOrDefault() != null)
            {
                return new ApiResult("This e-mail is already in use.", true);
            }

            //Get password bytes.
            byte[] pass = UTF8Encoding.UTF8.GetBytes(userRegister.password);

            //Convert hash and password to base64.
            string hash_pass;
            using (SHA512 sha = new SHA512Managed())
            {
                byte[] pass_with_salt = pass.Concat(this.SALT).ToArray();
                hash_pass = Convert.ToBase64String(sha.ComputeHash(pass_with_salt));
            }

            User user = new User()
            {
                email = userRegister.email,
                name = userRegister.name,
                password = hash_pass
            };

            //Save user
            this._db.Add(user);
            this._db.SaveChanges();

            return new ApiResult("User registerd.");
        }

        private ApiResult RefreshAccessToken(MRefreshToken refreshToken)
        {
            if (refreshToken.token.Length != 88)
            {
                return new ApiResult("Token length is invalid.", true);
            }

            AccessToken access_token = this._db.AccessTokens.Where(x => x.token == refreshToken.token && x.refresh_token != null).Include(x => x.refresh_token).Include(x => x.user).FirstOrDefault();
            if (access_token == null)
            {
                return new ApiResult("Token not found.", true);
            }

            //Remove old tokens for this user.
            //Remove all refresh tokens for this user.
            List<RefreshToken> OldRefreshTokens = this._db.RefreshTokens.Where(x => x.access_token.user_id == access_token.user_id || x.access_token == null).Include(x => x.access_token).ToList();
            this._db.RemoveRange(OldRefreshTokens);
            //Remove all access tokens for this user.
            List<AccessToken> OldAccessTokens = this._db.AccessTokens.Where(x => x.user_id == access_token.user_id).ToList();
            this._db.RemoveRange(OldAccessTokens);
            this._db.SaveChanges();

            //Check if the refresh token has expired.
            if (access_token.expires_at < DateTime.Now)
            {
                return new ApiResult("Token expired.", true);
            }

            //Generate new tokens.
            return this.GenerateTokens(access_token.user);
        }

        private ApiResult GenerateTokens(User user)
        {
            string uidToken = DateTime.Now.ToString("hh.mm.ss.ffffff") + user.email;
            string uidRefreshToken = DateTime.Now.ToString("hh.mm.ss.ffffff") + user.email + "_refresh";


            AccessToken accessToken = this.GenerateToken(user);
            AccessToken refreshAccessToken = this.GenerateToken(user, false);

            //Create refresh token model
            RefreshToken refreshToken = new RefreshToken()
            {
                access_token = refreshAccessToken,
                expires_at = refreshAccessToken.expires_at
            };

            this._db.Add(accessToken);
            this._db.Add(refreshAccessToken);

            this._db.Add(refreshToken);
            this._db.SaveChanges();

            object result = new
            {
                access_token = accessToken.token,
                access_token_expire =  accessToken.expires_at.ToString("MM-dd-yyyy HH:mm:ss"),
                refresh_token = refreshAccessToken.token,
                refresh_token_expire = refreshAccessToken.expires_at.ToString("MM-dd-yyyy HH:mm:ss"),
            };

            return new ApiResult(result);
        }

        private AccessToken GenerateToken(User user, bool is_access_token = true)
        {
            Random random = new Random();
            string uid = random.Next(0, 1000000) + DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss.ffffff") + user.email;
            uid += (is_access_token) ? "" : "_refresh";

            string str_token;
            using (SHA512 sha = new SHA512Managed())
            {
                str_token = Convert.ToBase64String(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(uid)));
            }

            AccessToken token = new AccessToken()
            {
                token = str_token,
                user_id = user.id,
                expires_at = DateTime.Now.AddMinutes((is_access_token) ? ACCESS_TOKEN_EXPIRE : REFRESH_TOKEN_EXPIRE)
            };
            return token;
        }
    }
}