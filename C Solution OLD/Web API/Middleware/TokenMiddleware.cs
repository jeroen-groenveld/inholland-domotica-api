using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.IO;
using Newtonsoft.Json;

namespace Web_API.Middleware
{
    public class TokenAuthorize
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<TokenMiddleware>();
        }
    }

    public class TokenMiddleware
    {
        private RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains("X-Not-Authorized"))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                using (var writer = new StreamWriter(context.Response.Body))
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(new Error("Unauthorized")));
                }
                return;
            }

            await _next.Invoke(context);
        }
    }

    public class Error
    {
        public string error;
        public Error(string err)
        {
            this.error = err;
        }
    }

}