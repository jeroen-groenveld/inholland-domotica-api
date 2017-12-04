using Hangfire;
using Domotica_API.Models;
using Microsoft.AspNetCore.Builder;

namespace Domotica_API.Schedules
{
    public static class Scheduler
    {
        public static void Run()
        {
            //Add your tasks here.

            //Do task hourly.
            RecurringJob.AddOrUpdate(() => new TokenCleaner().Run(), Cron.Hourly);
        }
    }
}
