using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Middleware;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web_API.Controllers;



namespace Web_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/user")]
    public class UserController : ApiController
    {
        //Constructor
        public UserController(DatabaseContext db) : base(db) { }

        [HttpGet]
        public ApiResult index()
        {
            return new ApiResult("end-point: user.");
        }

        [HttpGet("profile")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult GetProfile()
        {
            User user = (User)HttpContext.Items["user"];

            var profile = new
            {
                id = user.id,
                email = user.email,
                name = user.name,
                created_at = user.created_at,
                updated_at = user.updated_at
            };

            return new ApiResult(profile);
        }


		[HttpPost("register")]
		public ApiResult Register(UserRegister userRegister)
		{
			//try
			//{
			//	//Check if the model is valid.
			//	if (ModelState.IsValid == false)
			//	{
			//		return this.RegisterUser(userRegister);
			//	}
			//}
			//catch { }

			if (this._db.Users.Where(x => x.email == userRegister.email).FirstOrDefault() != null)
			{
				return new ApiResult("This e-mail is already in use.", true);
			}


			//Hash the password.
			string pass_hash = Convert.ToBase64String(this.HashPassword(userRegister.password));

			//Create new user instance.
			User user = new User()
			{
				email = userRegister.email,
				name = userRegister.name,
				password = pass_hash
			};

			//Save user
			this._db.Add(user);
			this._db.SaveChanges();

			return new ApiResult("User registerd.");
		}


		public User Authenticate(UserLogin userLogin)
		{
			User user = this._db.Users.Where(x => x.email == userLogin.email).FirstOrDefault();

			//Check if user exists
			if (user == null)
			{
				return null;
			}

			//Validate the given password against the user password.
			if (this.ValidatePassword(user.password, userLogin.password))
			{
				return user;
			}
			return null;
		}

		private bool ValidatePassword(string validPassword, string givenPassword)
		{
			//Get sha512 hash of the password stored in the database.
			byte[] db_pass_hash = Convert.FromBase64String(validPassword);

			//Hash the given password and check it against database password.
			byte[] given_pass_hash = this.HashPassword(givenPassword);

			//Check if the bytes of tha hashes are equal.
			if(db_pass_hash.SequenceEqual(given_pass_hash))
			{
				return true;
			}

			return false;
		}

		private byte[] HashPassword(string password)
		{
			using (SHA512 sha = new SHA512Managed())
			{
				byte[] pass = UTF8Encoding.UTF8.GetBytes(password);
				byte[] pass_with_salt = pass.Concat(UTF8Encoding.UTF8.GetBytes(env.APP_KEY)).ToArray();
				return sha.ComputeHash(pass_with_salt);
			}
		}
    }

	public class UserLogin
	{
		[Required]
		[MaxLength(50)]
		public string email { get; set; }

		[Required]
		[MaxLength(64)]
		public string password { get; set; }
	}

	public class UserRegister : UserLogin
	{
		[Required]
		[MaxLength(50)]
		public string name { get; set; }
	}
}