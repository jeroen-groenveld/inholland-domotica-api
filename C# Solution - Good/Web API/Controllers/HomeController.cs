﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult index()
        {
            return View("home");
        }
    }
}