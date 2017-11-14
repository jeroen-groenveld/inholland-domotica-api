using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class HeaterController : ApiController
    {
        const int HEATER_ID = 1;
        const int HEATER_MAX = 35;
        const int HEATER_MIN = 12;

        public IHttpActionResult Get()
        {
            House House = new House();

            // Read status
            string StatusHeater = House.GetState("heater");

            // Get / parse temp
            int Temperature = int.Parse( StatusHeater.Substring(StatusHeater.Length - 2) );

            Heater Heater = new Heater { id = HEATER_ID, temperature = Temperature };

            House.Close();
            return Json(Heater);
        }

        public IHttpActionResult Put(int id, int temperature)
        {
            House House = new House();

            if (temperature > HEATER_MAX || temperature < HEATER_MIN)
            {
                House.Close();
                return NotFound();
            }

            // Update status
            House.GetState("heater " + temperature);

            // Verify update
            string StatusHeater = House.GetState("heater");
            int Temperature = int.Parse(StatusHeater.Substring(StatusHeater.Length - 2));

            Heater Heater = new Heater { id = HEATER_ID, temperature = Temperature };

            House.Close();
            return Json(Heater);
        }
    }
}
