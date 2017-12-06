using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Domotica_API.Middleware;
using Domotica_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Domotica_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/background")]
    public class BackgroundController : ApiController
    {
        //Constructor
        public BackgroundController(DatabaseContext db) : base(db) { }

        [HttpGet]
        public IActionResult Show()
        {
            List<Background> backgrounds = db.Backgrounds.ToList();
            return Ok(backgrounds);
        }

        [HttpGet("{id}")]
        public IActionResult GetBackgroundById(int id)
        {
            Background background = db.Backgrounds.SingleOrDefault(x => x.id == id);
            if (background == null)
            {
                return BadRequest("Background does not exist");
            }

            return Ok(background);
        }
    }
}