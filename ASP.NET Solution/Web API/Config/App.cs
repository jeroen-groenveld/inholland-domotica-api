using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Config
{
    public static class App
    {
		public const string API_ROOT_PATH = "v1";

		public const int API_ACCESS_TOKEN_EXPIRE = 10;
		public const int API_REFRESH_TOKEN_EXPIRE = 30;
	}
}
