using Hangfire;
using Web_API.Models;

namespace Web_API.Scheduler
{
    public class Tasks
    {
        protected DatabaseContext _db;

        public Tasks(DatabaseContext db)
        {
            _db = db;
        }

        public void Run()
        {
            //Add your tasks here.

            //Do task hourly.
            RecurringJob.AddOrUpdate(() => new TokenCleaner(this._db).Run(), Cron.Hourly);
        }
    }
}
