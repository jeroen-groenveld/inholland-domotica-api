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
using Domotica_API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Routing;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.Dashboard;
using System.Configuration;
using Domotica_API.Models;
using Domotica_API.Schedules;
using Domotica_API.Seeds;
using Microsoft.EntityFrameworkCore;
using Domotica_API.Config;


namespace Domotica_API
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
            services.AddCors();
            services.AddDbContext<DatabaseContext>();
            services.AddHangfire(x => x.UseSqlServerStorage(env.CONNECTION_STRING));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .WithHeaders("accept", "content-type", "origin", "authorization")
            );

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "",
                    defaults: new { controller = "Home", action = "Index" });
            });

            app.UseHangfireDashboard("/tasks",
                new DashboardOptions {
                    Authorization = new [] { new HangfireAuth() }
                });
            app.UseHangfireServer();

            Scheduler.Run();
        }
    }
}
