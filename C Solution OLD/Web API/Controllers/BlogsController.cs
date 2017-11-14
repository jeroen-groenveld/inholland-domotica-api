using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Models;
using Microsoft.AspNetCore.Authorization;
using Web_API.Middleware;

namespace Web_API.Controllers
{
    [Produces("application/json")]
    [Route("api/blogs")]
    public class BlogsController : DefaultController
    {
        private readonly DatabaseContext _db;

        public BlogsController(DatabaseContext db)
        {
            _db = db;
        }

        // GET: api/Blogs


        [HttpGet]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public string[] GetBlogs()
        {
            return new string[] { "hey" };
            //return this.Pagination(this._db.Blogs);
        }

        [HttpGet("page/{page}")]
        public PaginationResult GetBlogs([FromRoute] int page)
        {
            return this.Pagination(this._db.Blogs, page);
        }

        [HttpGet("page/{page}/count/{count}")]
        public PaginationResult GetBlogs([FromRoute] int page, [FromRoute] int count)
        {
            return this.Pagination(this._db.Blogs, page, count);
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blog = await _db.Blogs.SingleOrDefaultAsync(m => m.BlogId == id);

            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }
    }
}