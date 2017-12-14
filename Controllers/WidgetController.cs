using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domotica_API.Controllers;
using Domotica_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Domotica_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/widget")]
    public class WidgetController : ApiController
    {
        //Constructor
        public WidgetController(DatabaseContext db) : base(db) { }

    }
}