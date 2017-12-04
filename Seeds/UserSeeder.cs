using System;
using Domotica_API.Models;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Controllers;

namespace Domotica_API.Seeds
{
    public class UserSeeder : Seeder
    {
        protected override async Task Seed(DatabaseContext db)
        {
            User newUserJeroen = new User
            {
                name = "Jeroen Groenveld",
                email = "jeroengroenveld@live.nl",
                background_id = 1,
                password = Convert.ToBase64String(UserController.HashPassword("Hoi12345"))
            };
            if (db.Users.SingleOrDefault(x => x.email == newUserJeroen.email) == null)
            {
                db.Add(newUserJeroen);
            }


            User newUserTest = new User
            {
                name = "Mr. Testing oh Test",
                email = "test@live.nl",
                background_id = 1,
                password = Convert.ToBase64String(UserController.HashPassword("Hoi12345"))
            };
            if (db.Users.SingleOrDefault(x => x.email == newUserTest.email) == null)
            {
                db.Add(newUserTest);
            }

            await db.SaveChangesAsync();
        }
    }
}
