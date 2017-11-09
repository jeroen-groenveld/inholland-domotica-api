using System;
using System.Threading.Tasks;
using Web_API.Models;
using System.Collections.Generic;
namespace Web_API.Seeds
{
    public abstract class Seeder
    {
        protected DatabaseContext _context;

        public Seeder(DatabaseContext context)
        {
            _context = context;
        }

        public virtual async Task Run()
        {

        }
    }
}
