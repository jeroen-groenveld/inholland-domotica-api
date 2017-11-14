using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class LampController : ApiController
    {

        public IHttpActionResult Get()
        {
            House House = new House();

            // Ask for lamps
            string numberOfLamps = House.GetState("lamps");

            // Create array for lamps
            Lamp[] Lamps = new Lamp[int.Parse(numberOfLamps)];

            // Ask status foreach lamp
            for(int index = 0; index < Lamps.Length; index++)
            {
                string OnOff = House.GetState("lamp " + index.ToString());
                string Description = House.GetState("whereis lamp " + index);

                int Status = OnOff.Contains("Off") ? 0 : 1;

                Lamps[index] = new Lamp { id = index, status = Status, description = Description };
            }

            House.Close();
            return Json(Lamps);
        }
    }
}
