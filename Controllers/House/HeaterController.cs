using Microsoft.AspNetCore.Mvc;
using System;
using Domotica_API.Middleware;

namespace Domotica_API.Controllers.House
{
    [Route(Config.App.API_ROOT_PATH + "/house/heater")]
    public class HeaterController : Controller
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

            double Temperature = house.GetHeaterTemperature();
            Heater result = new Heater { id = House.HEATER_ID, temperature = Temperature };

            //Close connection to the house.
            house.Close();

            return Ok(result);
        }

        [HttpPut("temperature")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Put([FromBody] Validators.House.Heater heater)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data");
            }

            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return BadRequest("Could not connect to DaHaus.");
            }

            // Update status
            house.SetHeaterTemperature(heater.temperature);

            // Verify update
            double Temperature = house.GetHeaterTemperature();

            Heater result = new Heater { id = House.HEATER_ID, temperature = Temperature };

            //Close connection to the house.
            house.Close();

            return Ok(result);
        }
    }
}
