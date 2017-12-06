using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Config
{
    public static class App
    {
		public const string API_ROOT_PATH = "v1";

		public const int API_ACCESS_TOKEN_EXPIRE = 1000;
		public const int API_REFRESH_TOKEN_EXPIRE = 3000;

        public const int GRID_SIZE_HEIGHT = 12;
        public const int GRID_SIZE_WIDTH = 12;
    }
}
