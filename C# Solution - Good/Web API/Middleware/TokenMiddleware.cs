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
        //private DatabaseContext _db;

        public TokenMiddleware(RequestDelegate next)
        {
            this._next = next;
            //this._db = dbcontext;
        }

        public async Task Invoke(HttpContext context, DatabaseContext db)
        {
            //Check if the Authorization header exists.
            if (context.Request.Headers.Keys.Contains("Authorization") == false)
            {
                await this.Unauthorized(context, "No Authorization header found.");
                return;
            }

            //Check Authorization type.
            string headerValue = context.Request.Headers["Authorization"];
            if (headerValue.Substring(0, 5) != "Token")
            {
                await this.Unauthorized(context, "Authorization header is invalid.");
                return;
            }

            //Get the token. Check if token length is valid.
            string str_token = headerValue.Substring(6);
            if(str_token.Length != 88)
            {
                await this.Unauthorized(context, "Token length is invalid.");
                return;
            }

            //Check if token exists.
            AccessToken token = db.AccessTokens.Where(x => x.token == str_token).Include(x => x.user).FirstOrDefault();
            if(token == null)
            {
                await this.Unauthorized(context, "Token not found.");
                return;
            }

            //Check if token is expired.
            if(token.expires_at < DateTime.Now)
            {
                await this.Unauthorized(context, "Token expired.");
                return;
            }

            //Finnally when everything is fine, add the user to the context. Now controllers can access the Authorized user.
            context.Items["user"] = token.user;

            await _next.Invoke(context);
        }

        private async Task Unauthorized(HttpContext context, string message = "")
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult("Unauthorized: " + message, true)));
        }
    }
}