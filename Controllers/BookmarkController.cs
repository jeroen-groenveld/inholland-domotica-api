using System;
using System.Collections.Generic;
using System.Linq;
using Domotica_API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Domotica_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Domotica_API.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route(Config.App.API_ROOT_PATH + "/bookmark")]
    public class BookmarkController : ApiController
    {
        //Constructor
        public BookmarkController(DatabaseContext db) : base(db){}

        [HttpGet("{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Get(int id)
        {
            Bookmark bookmark = this.db.Bookmarks.SingleOrDefault(x => x.id == id);
            if (bookmark == null)
            {
                return BadRequest("Bookmark does not exist");
            }

            User user = (User)HttpContext.Items["user"];
            if (bookmark.user_id != user.id)
            {
                return Unauthorized();
            }

            return Ok(bookmark);
        }


        [HttpPut("{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Update(int id, [FromBody] Validators.Bookmark newBookmark)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect url.");
            }

            Bookmark bookmark = this.db.Bookmarks.SingleOrDefault(x => x.id == id);
            if (bookmark == null)
            {
                return BadRequest("Bookmark does not exist");
            }

            User user = (User)HttpContext.Items["user"];
            if (bookmark.user_id != user.id)
            {
                return Unauthorized();
            }

            this.db.Entry(bookmark).CurrentValues.SetValues(newBookmark);
            this.db.SaveChanges();

            return Ok(this.db.Bookmarks.SingleOrDefault(x=> x.id == bookmark.id));
        }

        [HttpDelete("{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Delete(int id)
        {
            Bookmark bookmark = this.db.Bookmarks.SingleOrDefault(x => x.id == id);
            if (bookmark == null)
            {
                return BadRequest("Bookmark does not exist");
            }

            User user = (User)HttpContext.Items["user"];
            if (bookmark.user_id != user.id)
            {
                return Unauthorized();
            }

            this.db.Remove(bookmark);
            this.db.SaveChanges();

            return Ok("removed");
        }

    }
}