using Microsoft.AspNetCore.Mvc;
using System;
using Web_API.Middleware;

namespace Web_API.Controllers.House
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

        [HttpPut("temperature/{temperature}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public IActionResult Put(double temperature)
        {
            temperature = Math.Round(temperature, 1);

            if (temperature > House.HEATER_MAX || temperature < House.HEATER_MIN)
            {
                return BadRequest("Invalid temperature. Min: " + House.HEATER_MIN.ToString() + " Max: " + House.HEATER_MAX + ".");
            }

            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return BadRequest("Could not connect to DaHaus.");
            }

            // Update status
            house.SetHeaterTemperature(temperature);

            // Verify update
            double Temperature = house.GetHeaterTemperature();

            Heater result = new Heater { id = House.HEATER_ID, temperature = Temperature };

            //Close connection to the house.
            house.Close();

            return Ok(result);
        }
    }
}
