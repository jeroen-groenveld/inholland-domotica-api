using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class WindowController : ApiController
    {
        public IHttpActionResult Get()
        {
            House House = new House();

            // Ask for lamps
            string numberOfWindows = House.GetState("windows");

            // Create array for lamps
            Window[] Windows = new Window[int.Parse(numberOfWindows)];

            // Ask status foreach lamp
            for (int index = 0; index < Windows.Length; index++)
            {
                string OpenClose = House.GetState("window " + index.ToString());
                string Description = House.GetState("whereis window " + index);

                int Status = OpenClose.Contains("Close") ? 1 : 0;

                Windows[index] = new Window { id = index, status = Status, description = Description };
            }

            House.Close();
            return Json(Windows);
        }

        public IHttpActionResult Put(int id)
        {
            House House = new House();

            // Current state
            string OpenClose = House.GetState("window " + id);
            int Status = OpenClose.Contains("Close") ? 1 : 0;

            // Update state
            House.GetState("window " + id + (Status == 0 ? " close" : " open"));

            string Description = House.GetState("whereis window " + id);

            // Verify update
            OpenClose = House.GetState("window " + id);
            Status = OpenClose.Contains("Close") ? 0 : 1;

            Window Window = new Window { id = id, status = Status, description = Description };

            House.Close();
            return Json(Window);
        }
    }
}
