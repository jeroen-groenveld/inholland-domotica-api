using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web_API.Models;
using Web_API.Seeds;
using Web_API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Routing;
using Web_API.Scheduler;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.Dashboard;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Web_API.Config;

namespace Web_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddDbContext<DatabaseContext>();
            services.AddDbContext<DatabaseContext>();

            services.AddTransient<DatabaseSeeder>();
            services.AddTransient<Tasks>();
            services.AddHangfire(x => x.UseSqlServerStorage(Config.Database.CONNECTION_STRING));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DatabaseSeeder seeder, Tasks tasks)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "",
                    defaults: new { controller = "Home", action = "Index" });
            });

            //GlobalConfiguration.Configuration.UseSqlServerStorage();
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            tasks.Run();
            //Run seeder
            //seeder.Run().Wait();
        }
    }
}
