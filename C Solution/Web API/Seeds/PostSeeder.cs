using System.Collections.Generic;
using System.Threading.Tasks;
using Web_API.Models;
using System.Linq;
using System;

namespace Web_API.Seeds
{
    public class PostSeeder : DatabaseSeeder
    {
        public PostSeeder(DatabaseContext context) : base(context) { }

        public override async Task Run()
        {
            List<Post> blogs = new List<Post>();
            for (int i = 0; i < 40; i++)
            {
                blogs.Add(new Post()
                {
                    BlogId = 1,
                    Title = "Post Number: " + i.ToString(),
                    Content = "Lorem impsum"

                });
            }

            //test
            Blog blog = this._context.Blogs.Where(x => x.BlogId == 1).First();
            Console.WriteLine(blog.BlogId);
            Console.WriteLine(blog.Posts.Count());

            List<Post> posts = this._context.Posts.Where(x => x.BlogId == 1).ToList();
            Console.WriteLine(posts.Count());
            List<Post> test = blog.Posts.ToList();
            Console.WriteLine(test.Count);


            this._context.AddRange(blogs);
            await this._context.SaveChangesAsync();


        }
    }
}
