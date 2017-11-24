using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Middleware;

namespace Web_API.Controllers
{
    [Route(Config.Api.API_ROOT_PATH + "/user")]
    public class UserController : ApiController
    {
        //Constructor
        public UserController(DatabaseContext db) : base(db) { }

        [HttpGet]
        public ApiResult index()
        {
            return new ApiResult("end-point: user.");
        }

        [HttpGet("profile")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult GetProfile()
        {
            User user = (User)HttpContext.Items["user"];

            var profile = new
            {
                id = user.id,
                email = user.email,
                name = user.name,
                created_at = user.created_at,
                updated_at = user.updated_at
            };

            return new ApiResult(profile);
        }
    }
}