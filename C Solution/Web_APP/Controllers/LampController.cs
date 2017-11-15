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

        public IHttpActionResult Put(int id)
        {
            House House = new House();

            // Current state
            string OnOff = House.GetState("lamp " + id);
            int Status = OnOff.Contains("Off") ? 1 : 0;

            // Update state
            House.GetState("lamp " + id + (Status == 0 ? " off" : " on"));

            string Description = House.GetState("whereis lamp " + id);

            // Verify update
            OnOff = House.GetState("lamp " + id);
            Status = OnOff.Contains("Off") ? 1 : 0;

            Lamp Lamp = new Lamp { id = id, status = Status, description = Description };

            House.Close();
            return Json(Lamp);
        }
    }
}
