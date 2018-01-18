using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Config
{
    public static class App
    {
		public const string API_ROOT_PATH = "v1";

        //Value is given in Minutes.
		public const int API_ACCESS_TOKEN_EXPIRE = 5;
		public const int API_REFRESH_TOKEN_EXPIRE = 30;
    }
}
