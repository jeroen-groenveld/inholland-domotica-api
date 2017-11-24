using Hangfire.Dashboard;
using Web_API.Middleware;
using Web_API.Models;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace Web_API.Scheduler
{
    public class HangfireAuth : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Task<bool> task = TokenMiddleware.IsAuthicated(context.GetHttpContext(), db);
                task.Wait();

                return task.Result;
            }
        }
    }
}
