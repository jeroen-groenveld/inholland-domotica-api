using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domotica_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult index()
        {
            return View("home");
        }

		[Route(Config.App.API_ROOT_PATH)]
		public ActionResult AppIndex()
		{
			return View("app");
		}
    }
}