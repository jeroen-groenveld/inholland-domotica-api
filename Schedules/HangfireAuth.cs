using Hangfire.Dashboard;
using System;
using System.Threading.Tasks;
using System.Linq;
using Domotica_API.Middleware;
using Domotica_API.Models;


namespace Domotica_API.Schedules
{
    public class HangfireAuth : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //using (DatabaseContext db = new DatabaseContext())
            //{
            //    Task<bool> task = TokenMiddleware.IsAuthicated(context.GetHttpContext(), db);
            //    task.Wait();

            //    return task.Result;
            //}
            return true;
        }
    }
}
