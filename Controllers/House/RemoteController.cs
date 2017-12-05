using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Domotica_API.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.CodeAnalysis.Semantics;


namespace Domotica_API.Controllers.House
{
    [Route(Config.App.API_ROOT_PATH + "/house/remote")]
    [MiddlewareFilter(typeof(TokenAuthorize))]
    public class RemoteController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (System.IO.File.Exists(env.HOUSE_SCREENSHOT_FILE) == false)
            {
                return BadRequest("Screenshot file('"+env.HOUSE_SCREENSHOT_FILE+"') not found. ");
            }

            string imageB64 = System.IO.File.ReadAllText(env.HOUSE_SCREENSHOT_FILE);
            string imageLastModified = System.IO.File.GetLastWriteTime(env.HOUSE_SCREENSHOT_FILE).ToString();

            return Ok(new {image = imageB64, updated_at = imageLastModified});
        }
    }
}
