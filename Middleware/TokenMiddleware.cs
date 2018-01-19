using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using System.IO;
using Newtonsoft.Json;
using Domotica_API.Controllers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Domotica_API.Models;
using Domotica_API.Models.TokenAuth;
using Microsoft.EntityFrameworkCore;

namespace Domotica_API.Middleware
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
            User user = await Authenticate(context, db, true);
            if(user == null)
            {
                //Stop the request, and return response.
                return;
            }

            //Finnally when everything is fine, add the user to the context. Now controllers can access the Authorized user.
            context.Items["user"] = user;

            await _next.Invoke(context);
        }

        private static async Task<User> Authenticate(HttpContext context, DatabaseContext db, bool debug = false)
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

        private static async Task Unauthorized(HttpContext context, string message = "")
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = message }));
        }
    }
}