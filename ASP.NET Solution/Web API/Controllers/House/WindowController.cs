using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Models.House;
using Web_API.Middleware;

namespace Web_API.Controllers.House
{
    [Route(Config.App.API_ROOT_PATH + "/house/windows")]
    public class WindowController
    { 

        [HttpGet]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult Get()
        {
            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return new ApiResult("Could not connect to DaHaus.", true);
            }

            Item[] Items = house.GetList(House.WINDOW_CMD_NAME, House.WINDOW_CMD_LIST_NAME);

            //Close connection to the house.
            house.Close();
            return new ApiResult(Items);
        }

        [HttpPut("switch/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult Switch(int id)
        {
            return this.SetWindow(id, true);
        }

        [HttpPut("open/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult Open(int id)
        {
            return this.SetWindow(id, false, true);
        }

        [HttpPut("close/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult CLose(int id)
        {
            return this.SetWindow(id, false, false);
        }

        private ApiResult SetWindow(int id, bool Switch, bool open = false)
        {
            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return new ApiResult("Could not connect to DaHaus.", true);
            }

            string NewStatus = (open ? "open" : "close");
            //Check if the lamp exists.
            Item Item = house.SetItem(id, House.WINDOW_CMD_NAME, House.WINDOW_CMD_LIST_NAME, Switch, NewStatus);
            house.Close();

            if (Item == null)
            {
                return new ApiResult("Could not find item with this id('" + id + "').", true);
            }

            return new ApiResult(Item);
        }
    }
}
