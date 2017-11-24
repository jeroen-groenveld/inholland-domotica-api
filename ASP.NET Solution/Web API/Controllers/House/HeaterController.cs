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
    [Route(Config.App.API_ROOT_PATH + "/house/heater")]
    public class HeaterController
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

            double Temperature = house.GetHeaterTemperature();
            house.Close();

            return new ApiResult(new Heater { id = House.HEATER_ID, temperature = Temperature });
        }

        [HttpPut("temperature/{temperature}")]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult Put(double temperature)
        {
            temperature = Math.Round(temperature, 1);

            if (temperature > House.HEATER_MAX || temperature < House.HEATER_MIN)
            {
                return new ApiResult("Invalid temperature. Min: " + House.HEATER_MIN.ToString() + " Max: " + House.HEATER_MAX + ".");
            }

            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if (house.Connect() == false)
            {
                return new ApiResult("Could not connect to DaHaus.", true);
            }

            // Update status
            house.SetHeaterTemperature(temperature);

            // Verify update
            double Temperature = house.GetHeaterTemperature();
            house.Close();

            return new ApiResult(new Heater { id = House.HEATER_ID, temperature = Temperature });
        }
    }
}
