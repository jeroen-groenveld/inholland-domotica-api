using Domotica_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Seeds
{
    public class BookmarkSeeder : Seeder
    {
        protected override async Task Seed(DatabaseContext db)
        {
            if (db.Bookmarks.Any())
            {
                return;
            }

            db.AddRange(new object[]
            {
                new Bookmark()
                {
                    url = "https://www.google.nl",
                    user_id = 1
                },
                new Bookmark()
                {
                    url = "https://www.dumpert.nl",
                    user_id = 1
                },
                new Bookmark()
                {
                    url = "https://www.tweakers.nl",
                    user_id = 1
                },
                new Bookmark()
                {
                    url = "https://www.nu.nl",
                    user_id = 2
                },
            });

            await db.SaveChangesAsync();
        }
    }
}
