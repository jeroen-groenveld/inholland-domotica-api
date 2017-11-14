using System.Collections.Generic;
using System.Threading.Tasks;
using Web_API.Models;
using System.Linq;
using System;

namespace Web_API.Seeds
{
    public class PostSeeder : DatabaseSeeder
    {
        public PostSeeder(DatabaseContext db) : base(db) { }

        public override async Task Run()
        {
            for (int i = 0; i < 40; i++)
            {
                Post post = new Post()
                {
                    BlogId = 1,
                    Title = "Post Number: " + i.ToString(),
                    Content = "Lorem impsum"

                };
                this._db.Add(post);
            }
            await this._db.SaveChangesAsync();
        }
    }
}
