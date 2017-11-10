using System.Collections.Generic;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.Seeds
{
    public class BlogSeeder : DatabaseSeeder
    {
        public BlogSeeder(DatabaseContext db) : base(db) { }

        public override async Task Run()
        {

            for(int i = 0; i < 500; i++)
            {
                Blog blog = new Blog()
                {
                    Url = "http://www.google.nl/",
                    Name = "Blog: " + i.ToString()

                };
                this._db.Add(blog);
            }

            await this._db.SaveChangesAsync();
        }
    }
}
