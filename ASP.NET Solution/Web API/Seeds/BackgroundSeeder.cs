using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.Seeds
{
    public class BackgroundSeeder : Seeder
    {
        protected override async Task Seed(DatabaseContext db)
        {
            Background exist_background = db.Backgrounds.Where(x => x.id == 1).SingleOrDefault();
            if (exist_background != null) { return; }

            Background background = new Background
            {
                name = "Background 1",
                description = "Beautifull background",
                data = "https://i.imgur.com/Ee4rX.jpg"
            };

            db.Add<Background>(background);
            await db.SaveChangesAsync();
        }
    }
}
