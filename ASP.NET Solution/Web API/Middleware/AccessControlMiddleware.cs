using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Web_API.Models;
using System;
using Microsoft.AspNetCore.Builder;

namespace Web_API.Middleware
{
    public class AccessControlMiddleware
    {
        private RequestDelegate _next;

        public AccessControlMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.IsApi())
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            await _next.Invoke(context);
        }
    }

    #region PathString IsApi Extension
    public static class IsApiExtension
    {
        public static bool IsApi(this PathString path)
        {
            string _path = path.ToString();
            if(_path.Length >= Config.Api.API_ROOT_PATH.Length + 2 && _path.Substring(0, Config.Api.API_ROOT_PATH.Length + 2) == "/"+ Config.Api.API_ROOT_PATH + "/")
            {
                return true;
            }
            return false;
        }
    }
    #endregion

    #region ExtensionMethod
    public static class AccessControlMiddlewareExtension
    {
        public static IApplicationBuilder ApplyAccessControl(this IApplicationBuilder app)
        {
            app.UseMiddleware<AccessControlMiddleware>();
            return app;
        }
    }
    #endregion
}
