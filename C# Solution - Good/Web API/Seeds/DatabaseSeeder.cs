using System;
using System.Threading;
using System.Threading.Tasks;
using Web_API.Models;
using Web_API.Models.TokenAuth;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Web_API.Seeds
{
    public class DatabaseSeeder : Seeder
    {
        public DatabaseSeeder(DatabaseContext db) : base(db) { }

        public override async Task Run()
        {
            //await this.StartSeeder(new BlogSeeder(this._db), "Blog Seeder");

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
