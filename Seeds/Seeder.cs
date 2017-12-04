using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Models;

namespace Domotica_API.Seeds
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
