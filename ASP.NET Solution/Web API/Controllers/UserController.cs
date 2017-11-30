using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using Web_API.Middleware;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/user")]
    public class UserController : ApiController
    {
        //Constructor
        public UserController(DatabaseContext _db) : base(_db) { }

        [HttpGet]
        public IActionResult index()
        {
            return Ok("end-point: user.");
        }

        [HttpGet("profile")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetProfile()
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

            return Ok(profile);
        }

        [HttpGet("background")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetBackground()
        {
            User user = (User)HttpContext.Items["user"];
            Background background = this.db.Users.Where(x => x.id == user.id).Include(x => x.background).FirstOrDefault().background;

            var result = new
            {
                id = background.id,
                name = background.name,
                description = background.description,
                data = background.data
            };

            return Ok(result);
        }

        [HttpGet("widgets")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetWidgets()
        {
            User user = (User)HttpContext.Items["user"];

            List<ActiveWidget> activeWidgets = user.ActiveWidgets.ToList();

            List<object> result = new List<object>();
            foreach(ActiveWidget aw in activeWidgets)
            {
                var resultItem = new
                {
                    position = aw.position,
                    widget = new
                    {
                        name = aw.widget.name,
                        description = aw.widget.description,
                        width = aw.widget.size_w,
                        height = aw.widget.size_h
                    }
                };

                result.Add(resultItem);
            }

            return Ok(result);
        }

        [HttpGet("bookmarks")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetBookmarks()
        {
            User user = (User)HttpContext.Items["user"];

            List<Bookmark> bookmarks = user.Bookmarks.ToList();

            List<object> result = new List<object>();
            foreach (Bookmark bookmark in bookmarks)
            {
                var resultItem = new
                {
                    name = bookmark.name,
                    url = bookmark.url,
                    created_at = bookmark.created_at,
                    updated_at = bookmark.updated_at
                };

                result.Add(resultItem);
            }

            return Ok(result);
        }


        [HttpPost("register")]
		public IActionResult Register([FromForm] UserRegister userRegister)
		{
            if(ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            if (this.db.Users.Where(x => x.email == userRegister.email).FirstOrDefault() != null)
			{
				return BadRequest("This e-mail is already in use.");
			}

			//Hash the password.
			string pass_hash = Convert.ToBase64String(HashPassword(userRegister.password));

            //Create new user instance.
            User user = new User()
            {
                email = userRegister.email,
                name = userRegister.name,
                password = pass_hash,
                background_id = 1,
            };

            //Save user
            this.db.Add(user);
            this.db.SaveChanges();

            return Ok("User registerd.");
		}

        public static User Authenticate(UserLogin userLogin)
		{
            using (DatabaseContext db = new DatabaseContext())
            {
                User user = db.Users.Where(x => x.email == userLogin.email).FirstOrDefault();

                //Check if user exists
                if (user == null)
                {
                    return null;
                }

                //Validate the given password against the user password.
                if (ValidatePassword(userLogin.password, user.password))
                {
                    return user;
                }
                return null;
            }
		}

		private static bool ValidatePassword(string givenPassword, string databasePasswordHash)
		{
			//Retrieve password hash and salt from databae password.
			byte[] db_pass_hash = Convert.FromBase64String(databasePasswordHash);
            byte[] db_pass_hash_hash = db_pass_hash.Take(Config.Hash.PASSWORD_HASH_SIZE / 8).ToArray();
            byte[] db_pass_hash_salt = db_pass_hash.Skip(Config.Hash.PASSWORD_HASH_SIZE / 8).ToArray();


            //Hash the given password and with the salt from the database.
            byte[] given_pass_hash = HashPassword(givenPassword, db_pass_hash_salt);

            //Check if the bytes of the hashes are equal.
            if (db_pass_hash.SequenceEqual(given_pass_hash))
            {
                return true;
            }

            return false;
		}

		private static byte[] HashPassword(string password, byte[] salt = null)
		{
            //Check if salt is given, if not generate new one. This would only occur when generating a new password for a user that's registering.
            if(salt == null)
            {
                //Generate salt.
                salt = new byte[Config.Hash.PASSWORD_HASH_SALT_SIZE / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            //Generate hash
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: Config.Hash.PASSWORD_HASH_ITERATIONS,
                numBytesRequested: Config.Hash.PASSWORD_HASH_SIZE / 8);

            //Concatenate hash and salt.
            byte[] new_hash = hash.Concat(salt).ToArray();


            Console.WriteLine("salt: " + Convert.ToBase64String(salt));
            Console.WriteLine("hash: " + Convert.ToBase64String(hash));
            Console.WriteLine("new hash length: " + new_hash.Length.ToString());
            Console.WriteLine("Should be hash: " + Convert.ToBase64String(new_hash.Take(512 / 8).ToArray()));
            Console.WriteLine("Should be salt: " + Convert.ToBase64String(new_hash.Skip(512 / 8).ToArray()));

            return new_hash;
        }
    }

	public class UserLogin
	{
		[Required]
		[MaxLength(50)]
		public string email { get; set; }

		[Required]
		[MaxLength(64), MinLength(8)]
		public string password { get; set; }
	}

	public class UserRegister : UserLogin
	{
		[Required]
		[MaxLength(50)]
        public string name { get; set; }
	}
}