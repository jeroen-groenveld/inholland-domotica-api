using System.Collections.Generic;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.Seeds
{
    public class BlogSeeder : DatabaseSeeder
    {
        public BlogSeeder(DatabaseContext context) : base(context) { }

        public override async Task Run()
        {
            List<Blog> blogs = new List<Blog>();
            for(int i = 0; i < 500; i++)
            {
                blogs.Add(new Blog()
                {
                    Url = "http://www.google.nl/",
                    Name = "Blog: " + i.ToString()

                });
            }

            this._context.AddRange(blogs);
            await this._context.SaveChangesAsync();
        }
    }
}
