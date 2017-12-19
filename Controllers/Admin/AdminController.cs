using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Controllers;
using Domotica_API.Middleware;
using Domotica_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domotica_API.Controllers.Admin
{
    [Route(Domotica_API.Config.App.API_ROOT_PATH + "/admin")]
    [MiddlewareFilter(typeof(TokenAuthorize))]
    public class BackgroundController : ApiController
    {
        //Constructor
        public BackgroundController(DatabaseContext db) : base(db) { }


        [HttpGet("userlist/{page?}/{amount?}")]
        public IActionResult GetUserList(int page = DEFAULT_PAGE, int amount = DEFAULT_AMOUNT)
        {
            User user = (User)HttpContext.Items["user"];
            if (user.is_admin == false)
            {
                return Unauthorized();
            }

            List<User> users = this.db.Users.OrderBy(x => x.is_admin).ToList();
            users.Reverse();

            PaginationResult result = this.Pagination(users, page, amount);

            return Ok(result);
        }
   
    }
}