using Domotica_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Seeds
{
    public class BackgroundSeeder : Seeder
    {
        protected override async Task Seed(DatabaseContext db)
        {
            if (db.Backgrounds.SingleOrDefault(x => x.id == 1) == null)
            {
                db.Add(new Background
                {
                    name = "Mountain by the lake",
                    description = "",
                    data = "https://inholland.it/static/backgrounds/mountain-lake.jpg"
                });
            }

            if (db.Backgrounds.SingleOrDefault(x => x.id == 2) == null)
            {
                db.Add(new Background
                {
                    name = "Mountain",
                    description = "",
                    data = "https://inholland.it/static/backgrounds/mountain.jpg"
                });
            }

            if (db.Backgrounds.SingleOrDefault(x => x.id == 3) == null)
            {
                db.Add(new Background
                {
                    name = "Mountain road",
                    description = "",
                    data = "https://inholland.it/static/backgrounds/road.jpg"
                });
            }

            if (db.Backgrounds.SingleOrDefault(x => x.id == 4) == null)
            {
                db.Add(new Background
                {
                    name = "Forest",
                    description = "",
                    data = "https://inholland.it/static/backgrounds/forest.jpg"
                });
            }

            await db.SaveChangesAsync();
        }
    }
}
