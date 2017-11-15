using System;
using System.Threading.Tasks;
using Web_API.Models;
using Web_API.Models.TokenAuth;
using System.Diagnostics;

namespace Web_API.Seeds
{
    public class DatabaseSeeder : Seeder
    {
        public DatabaseSeeder(DatabaseContext db) : base(db) { }

        public override async Task Run()
        {
            User user = new User()
            {
                email = "jeroengroenveld@live.nl",
                name = "Jeroen Groenveld",
                password = "asdf"
            };
            this._db.Add(user);
            this._db.SaveChanges();

            Token token = new Token()
            {
                token = "Z9Zp9FjWoOp+O+XalMkGpDUY9CFwDQ733hPcHbici/v6f5+ARRQ9GEKgpbkAf1q2rFRXDC8zD+wNByx+oVjKOg==",
                user_id = 1,
                expires_at = DateTime.Now.AddHours(1)
            };
            this._db.Add(token);
            this._db.SaveChanges();

            //await this.StartSeeder(new BlogSeeder(this._db), "Blog Seeder");
            //await this.StartSeeder(new PostSeeder(this._db), "Post Seeder");
        }

        private async Task StartSeeder(Seeder seeder, string name = "")
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            await seeder.Run();
            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Console.WriteLine(name + ": " + elapsedTime);
        }
    }
}
