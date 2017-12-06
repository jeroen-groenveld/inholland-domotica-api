using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using Domotica_API.Middleware;
using Domotica_API.Models;
using Bookmark = Domotica_API.Models.Bookmark;

namespace Domotica_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/user")]
    public class UserController : ApiController
    {
        //Constructor
        public UserController(DatabaseContext db) : base(db) { }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("end-point: user.");
        }

        [HttpGet("profile/all")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetProfileAll()
        {
            User user = (User)HttpContext.Items["user"];
            Background background = db.Backgrounds.SingleOrDefault(x => x.id == user.background_id);
            List<Bookmark> bookmarks = db.Bookmarks.Where(x => x.user_id == user.id).ToList();

            var profile = new
            {
                user = user,
                background = background,
                bookmarks = bookmarks
            };

            return Ok(profile);
        }

        [HttpGet("profile")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetProfile()
        {
            User user = (User)HttpContext.Items["user"];

            return Ok(user);
        }

        [HttpPut("profile")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult UpdateProfile([FromBody] Validators.User.UserProfile userProfile)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            User user = (User)HttpContext.Items["user"];

            //Update background when found.
            Background background = db.Backgrounds.SingleOrDefault(x => x.id == userProfile.background_id);
            if (background == null)
            {
                return BadRequest("Background does not exist.");
            }
            else
            {
                user.background_id = background.id;
            }

            //Update name.
            user.name = userProfile.name;

            //Save user changes.
            this.db.SaveChanges();

            return Ok(user);
        }

        [HttpGet("background")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetBackground()
        {
            User user = (User)HttpContext.Items["user"];
            Background background = db.Backgrounds.SingleOrDefault(x => x.id == user.background_id);

            return Ok(background);
        }

        [HttpGet("bookmarks")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetBookmarks()
        {
            User user = (User)HttpContext.Items["user"];
            List<Bookmark> bookmarks = db.Bookmarks.Where(x => x.user_id == user.id).ToList();

            return Ok(bookmarks);
        }

        [HttpPost("register")]
		public IActionResult Register([FromBody] Validators.User.UserRegister userRegister)
		{
            if(ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            if (this.db.Users.SingleOrDefault(x => x.email == userRegister.email) != null)
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

        public static User Authenticate(Validators.User.UserLogin userLogin)
		{
            using (DatabaseContext db = new DatabaseContext())
            {
                User user = db.Users.SingleOrDefault(x => x.email == userLogin.email);

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

        public static bool ValidatePassword(string givenPassword, string databasePasswordHash)
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

		public static byte[] HashPassword(string password, byte[] salt = null)
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


            //Console.WriteLine("salt: " + Convert.ToBase64String(salt));
            //Console.WriteLine("hash: " + Convert.ToBase64String(hash));
            //Console.WriteLine("new hash length: " + new_hash.Length.ToString());
            //Console.WriteLine("Should be hash: " + Convert.ToBase64String(new_hash.Take(512 / 8).ToArray()));
            //Console.WriteLine("Should be salt: " + Convert.ToBase64String(new_hash.Skip(512 / 8).ToArray()));

            return new_hash;
        }
    }
}