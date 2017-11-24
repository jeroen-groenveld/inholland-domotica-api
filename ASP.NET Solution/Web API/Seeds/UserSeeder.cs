using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.Seeds
{
    public class UserSeeder : Seeder
    {
        protected override async Task Seed(DatabaseContext db)
        {
            User exist_user = db.Users.Where(x => x.email == "jeroen.groenveld@j20.nl").SingleOrDefault();
            if (exist_user != null) { return; }

            User user = new User {
                email = "jeroen.groenveld@j20.nl",
                name = "Test123",
                password = "4SnBlWfr5pCG/H+QjFnF2Gh5DK+GVEyYp0bc4em3wFDGnkpK7s5IB7GC5brm0nXpX5p870KDxd4/56RPCe91Xw=="
            };

            db.Add<User>(user);
            await db.SaveChangesAsync();
        }
    }
}
