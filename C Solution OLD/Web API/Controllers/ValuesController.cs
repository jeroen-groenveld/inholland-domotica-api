using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Middleware;

namespace Web_API.Controllers
{
    [MiddlewareFilter(typeof(TokenAuthorize))]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private DatabaseContext _db;

        public ValuesController(DatabaseContext db)
        {
            this._db = db;
        }

        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            return Json(new string[] { "adsf" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
