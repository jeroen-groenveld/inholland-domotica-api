using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Controllers
{
    public class ApiResult
    {
        public object result;
        public bool error;

        public ApiResult(object _result, bool _error = false)
        {
            this.result = _result == null ? "" : _result;
            this.error = _error;
        }
    }
}
