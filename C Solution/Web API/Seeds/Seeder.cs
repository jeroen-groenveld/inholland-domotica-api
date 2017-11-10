using System;
using System.Threading.Tasks;
using Web_API.Models;
using System.Collections.Generic;
namespace Web_API.Seeds
{
    public abstract class Seeder
    {
        protected DatabaseContext _db;

        public Seeder(DatabaseContext db)
        {
            _db = db;
        }

        public async virtual Task Run()
        {
            return;
        }
    }
}
