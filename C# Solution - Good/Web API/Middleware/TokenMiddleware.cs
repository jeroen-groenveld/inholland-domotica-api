using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.IO;
using Newtonsoft.Json;
using Web_API.Models;
using Web_API.Models.TokenAuth;
using Web_API.Controllers;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public async Task Invoke(HttpContext context, DatabaseContext db)
        {
            Console.WriteLine("LOL");

            User user = await Authenticate(context, db, true);
            if(user == null)
            {
                return;
            }

            context.Items["user"] = user;

            //Finnally when everything is fine, add the user to the context. Now controllers can access the Authorized user.
            await _next.Invoke(context);
        }

        private static async Task<User> Authenticate(HttpContext context, DatabaseContext db, bool Debug = false)
        {
            //Check if the Authorization header exists.
            if (context.Request.Headers.Keys.Contains("Authorization") == false)
            {
                await Unauthorized(context, "No Authorization header found.");
                return null;
            }

            //Check Authorization type.
            string headerValue = context.Request.Headers["Authorization"];
            if (headerValue.Substring(0, 5) != "Token")
            {
                await Unauthorized(context, "Authorization header is invalid.");
                return null;
            }

            //Get the token. Check if token length is valid.
            string str_token = headerValue.Substring(6);
            if (str_token.Length != 88)
            {
                await Unauthorized(context, "Token length is invalid.");
                return null;
            }

            //Check if token exists.
            AccessToken token = db.AccessTokens.Where(x => x.token == str_token).Include(x => x.user).FirstOrDefault();
            if (token == null)
            {
                await Unauthorized(context, "Token not found.");
                return null;
            }

            //Check if token is expired.
            if (token.expires_at < DateTime.Now)
            {
                await Unauthorized(context, "Token expired.");
                return null;
            }

            return token.user;
        }

        public static async Task<bool> IsAuthicated(HttpContext context, DatabaseContext db, bool Debug = false)
        {
            if(await Authenticate(context, db) != null)
            {
                return true;
            }

            return false;
        }

        private static async Task Unauthorized(HttpContext context, string message = "", bool Debug = false)
        {
            if(Debug)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult("Unauthorized: " + message, true)));
            }
        }
    }
}