using System;
using System.Threading.Tasks;
using Web_API.Models;
using System.Collections.Generic;
namespace Web_API.Seeds
{
    public abstract class Seeder
    {
        public async Task Run()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                await this.Seed(db);
            }
        }

        protected virtual async Task Seed(DatabaseContext db) { await Task.CompletedTask; }
    }
}
