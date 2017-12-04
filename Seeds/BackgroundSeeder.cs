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
                    name = "Background 1",
                    description = "Beautifull background",
                    data = "https://i.imgur.com/Ee4rX.jpg"
                });
            }

            if (db.Backgrounds.SingleOrDefault(x => x.id == 2) == null)
            {
                db.Add(new Background
                {
                    name = "Background 2",
                    description = "Beautifull background",
                    data = "https://i.imgur.com/Ee4rX.jpg"
                });
            }

            await db.SaveChangesAsync();
        }
    }
}
