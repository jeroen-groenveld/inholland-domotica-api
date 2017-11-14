using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Web_API.Middleware
{
    public class TokenAuthorize
    {
        public TokenAuthorize()
        {

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<TokenMiddleware>();
        }
    }

    public class TokenMiddleware
    {
        private RequestDelegate _next;

        public void AuthorizationMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains("X-Cancel-Request"))
            {
                context.Response.StatusCode = 500;
                return;
            }

            await _next.Invoke(context);

            if (context.Request.Headers.Keys.Contains("X-Transfer-By"))
            {
                context.Response.Headers.Add("X-Transfer-Success", "true");
            }
        }
    }
}