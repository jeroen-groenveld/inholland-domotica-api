using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Produces("application/json")]
    public class ApiController : Controller
    {
        protected readonly DatabaseContext _db;

        public ApiController(DatabaseContext db)
        {
            _db = db;
        }

        public class ApiResult
        {
            public object result;
            public string error;

            public ApiResult(object _result, string _error = "none")
            {
                this.result = _result == null ? "" : _result;
                this.error = _error;
            }
        }
    }
}