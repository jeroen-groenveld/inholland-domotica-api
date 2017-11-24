using Hangfire;
using Web_API.Models;

namespace Web_API.Scheduler
{
    public static class Tasks
    {
        public static void Run()
        {
            //Add your tasks here.

            //Do task hourly.
            RecurringJob.AddOrUpdate(() => new TokenCleaner().Run(), Cron.Hourly);
        }
    }
}
