using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Domotica_API.Models;

namespace Domotica_API.Controllers.House
{
    [Route(Config.App.API_ROOT_PATH + "/house/windows")]
    public class WindowController : Controller
    { 

        [HttpGet]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Get()
        {
            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return BadRequest("Could not connect to DaHaus.");
            }

            Item[] Items = house.GetList(House.WINDOW_CMD_NAME, House.WINDOW_CMD_LIST_NAME);

            //Close connection to the house.
            house.Close();

            return Ok(Items);
        }

        [HttpPut("switch/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Switch(int id)
        {
            return this.SetWindow(id, true);
        }

        [HttpPut("open/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Open(int id)
        {
            return this.SetWindow(id, false, true);
        }

        [HttpPut("close/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult CLose(int id)
        {
            return this.SetWindow(id, false, false);
        }

        private IActionResult SetWindow(int id, bool Switch, bool open = false)
        {
            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return BadRequest("Could not connect to DaHaus.");
            }

            string NewStatus = (open ? "open" : "close");
            //Check if the lamp exists.
            Item Item = house.SetItem(id, House.WINDOW_CMD_NAME, House.WINDOW_CMD_LIST_NAME, Switch, NewStatus);
            house.Close();

            if (Item == null)
            {
                return BadRequest("Could not find item with this id('" + id + "').");
            }

            return Ok(Item);
        }
    }
}
