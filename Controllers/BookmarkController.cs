using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Domotica_API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Domotica_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Domotica_API.Controllers
{
    [MiddlewareFilter(typeof(TokenAuthorize))]
    [Microsoft.AspNetCore.Mvc.Route(Config.App.API_ROOT_PATH + "/bookmark")]
    public class BookmarkController : ApiController
    {
        private const string URL_REGEX = "^((?:https?\\:\\/\\/|www\\.)(?:[-a-z0-9]+\\.)*[-a-z0-9]+.*)$";

        //Constructor
        public BookmarkController(DatabaseContext db) : base(db){}

        [HttpGet]
        public IActionResult Show(int id)
        {
            User user = (User)HttpContext.Items["user"];

            return Ok(this.GetUserBookmarks(user));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Validators.Bookmark newBookmark)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            Regex urlRegex = new Regex(URL_REGEX);
            if (urlRegex.IsMatch(newBookmark.url) == false)
            {
                return BadRequest("This url is not valid.");
            }

            User user = (User)HttpContext.Items["user"];
            this.db.Add(new Bookmark
            {
                name = newBookmark.name,
                url = newBookmark.url,
                user_id = user.id
            });

            this.db.SaveChanges();
            return Ok(this.GetUserBookmarks(user));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Validators.Bookmark newBookmark)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data.");
            }

            Regex urlRegex = new Regex(URL_REGEX);
            if (urlRegex.IsMatch(newBookmark.url) == false)
            {
                return BadRequest("This url is not valid.");
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

            return Ok(this.GetUserBookmarks(user));
        }

        [HttpDelete("{id}")]
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

            return Ok(this.GetUserBookmarks(user));
        }

        public List<Bookmark> GetUserBookmarks(User user)
        {
            return this.db.Bookmarks.Where(x => x.user_id == user.id).ToList();
        }
    }
}