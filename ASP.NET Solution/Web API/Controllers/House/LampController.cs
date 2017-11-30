using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Middleware;

namespace Web_API.Controllers.House
{
    [Route(Config.App.API_ROOT_PATH + "/house/lamps")]
    public class LampController : Controller
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

            Item[] Items = house.GetList(House.LAMP_CMD_NAME, House.LAMP_CMD_LIST_NAME);

            //Close connection to the house.
            house.Close();

            return Ok(Items);
        }

        [HttpPut("switch/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Switch(int id)
        {
            return this.SetLight(id, true);
        }

        [HttpPut("on/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult On(int id)
        {
            return this.SetLight(id, false, true);
        }

        [HttpPut("off/{id}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Off(int id)
        {
            return this.SetLight(id, false, false);
        }

        private IActionResult SetLight(int id, bool Switch, bool on = false)
        {
            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return BadRequest("Could not connect to DaHaus.");
            }

            string NewStatus = (on ? "on" : "off");            
            //Check if the lamp exists.
            Item Item = house.SetItem(id, House.LAMP_CMD_NAME, House.LAMP_CMD_LIST_NAME, Switch, NewStatus);

            //Close connection to the house.
            house.Close();

            if (Item == null)
            {
                return BadRequest("Could not find item with this id('"+id+"').");
            }

            return Ok(Item);
        }
    }
}
