using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Domotica_API.Middleware;
using Domotica_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.AzureAppServices.Internal;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            user.background_id = background.id;
            

            //Update name.
            user.name = userProfile.name;

            //Save user changes.
            this.db.SaveChanges();

            return Ok(user);
        }

        [HttpGet("profile/image")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult GetProfileImage()
        {
            User user = (User)HttpContext.Items["user"];

            string user_profile_image = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/profiles/" + user.id + "/" + user.image;

            if (System.IO.File.Exists(user_profile_image) == false)
            {
                if (string.IsNullOrEmpty(user.image) == false)
                {
                    user.image = null;
                    this.db.SaveChanges();
                }
                return BadRequest("No profile image found.");
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(user_profile_image);
            return File(imageBytes, this.GetFileMime(user.image));
        }

        [HttpPost("profile/image")]
	    [MiddlewareFilter(typeof(TokenAuthorize))]
		public async Task<IActionResult> Post(IFormFile file)
	    {
            //Check if the request contains a file.
		    if (file == null || file.Length == 0)
		    {
			    return BadRequest("No file found.");
			}


		    string[] allowedTypes = new string[] { "image/jpeg", "image/png" };

			//Check the contentype of the file, can either be JPEG or PNG.
		    if (allowedTypes.Contains(file.ContentType) == false)
		    {
			    return BadRequest("Only PNG and JPG are permitted.");
		    }

			//Max 2Mb file size.
		    if (file.Length > 2097152)
		    {
			    return BadRequest("Only files smaller than 2MB are permitted.");
		    }

		    string filename = "profile-image." + this.GetMimeExtension(file.ContentType);

			User user = (User)HttpContext.Items["user"];
			string user_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/profiles/" + user.id;
		    var file_path = Path.Combine(user_path, filename);

			//Check if the user folder exists, if not create it.
			if (Directory.Exists(user_path) == false)
			{
				Directory.CreateDirectory(user_path);
			}

			//If the user already has an image. Delete the old one.
		    if (user.image != null)
		    {
				System.IO.File.Delete(Path.Combine(user_path, user.image));
		    }

			//Write new image to disk.
		    using (var stream = new FileStream(file_path, FileMode.Create))
		    {
			    await file.CopyToAsync(stream);
		    }

			//Save new image to the DB.
		    user.image = filename;
		    this.db.SaveChanges();

			return Ok("File uploaded.");
	    }

	    private string GetFileMime(string file)
	    {
		    Dictionary<string, string> mimes = new Dictionary<string, string>
		    {
			    {".jpg", "image/jpeg"},
			    {".png", "image/png"}
		    };

		    string extension = Path.GetExtension(file);

		    return mimes[extension];
	    }

	    private string GetMimeExtension(string mime)
	    {
		    Dictionary<string, string> extensions = new Dictionary<string, string>
		    {
			    {"image/jpeg", ".jpg"},
			    {"image/png", ".png"}
		    };

		    return extensions[mime];
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
            //Check post data.
            if(ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            //Check if a user is already registrated with this e-mail.
            if (this.db.Users.SingleOrDefault(x => x.email == userRegister.email) != null)
			{
				return BadRequest("This e-mail is already in use.");
			}

			//Hash the password and convert it to a Base64 string.
			string pass_hash = Convert.ToBase64String(HashPassword(userRegister.password));

            //Create new user instance.
            User user = new User()
            {
                email = userRegister.email,
                name = userRegister.name,
                password = pass_hash,
                background_id = 1,
            };

            //Send register mail
            this.SendRegisterMail(user);

            //Save user
            this.db.Add(user);
            this.db.SaveChanges();

            return Ok("User registerd.");
		}

        private void SendRegisterMail(User user)
        {
            var msg = new SendGridMessage();
            msg.AddTo(user.email, user.name);
            msg.From = new EmailAddress("info@inholland.it", "Inholland Domotica Project");
            msg.Subject = "Registration";

            string html = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Email/register.html");
            html = html.Replace("[$name]", user.name);

            msg.HtmlContent = html;
            msg.PlainTextContent = "Hi " + user.name + ". Thanks for registering!";

            var transport = new SendGridClient(env.SENDGRID_API_KEY);

            //Send the message Async so the request doesn't wait untill the e-mail is sent.
            transport.SendEmailAsync(msg);
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
                    //If passwords match then return the user.
                    return user;
                }
                return null;
            }
		}

        public static bool ValidatePassword(string givenPassword, string databasePasswordHash)
		{
			//Convert the Password & Salt Base64 hash to a byte array.
			byte[] db_passhash_with_salt = Convert.FromBase64String(databasePasswordHash);
            //Take the Salt from the byte array.
            byte[] db_salt = db_passhash_with_salt.Skip(Config.Hash.PASSWORD_HASH_SIZE / 8).ToArray();


            //Hash the given password with the salt from the database.
            byte[] given_passhash_with_salt = HashPassword(givenPassword, db_salt);


            //Check if the Database Hash is equal to the Given Password Hash.
            if (db_passhash_with_salt.SequenceEqual(given_passhash_with_salt))
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

            //Concatenate the salt after the hash.
            byte[] new_hash = hash.Concat(salt).ToArray();

            return new_hash;
        }
	}
}