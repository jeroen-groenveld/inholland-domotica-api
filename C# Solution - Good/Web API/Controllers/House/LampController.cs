using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Middleware;

namespace Web_API.Controllers.House
{
    [Route("api/house/lamps")]
    public class LampController : ApiController
    {
        //Constructor
        public LampController(DatabaseContext db) : base(db) { }

        public class Lamp
        {
            public int id { get; set; }
            public bool on { get; set; }
            public int floor { get; set; }
            public string location { get; set;  }
        }

        [HttpGet]
        [MiddlewareFilter(typeof(TokenAuthorize))]
        public ApiResult Get()
        {
            House house = new House();

            //Connect to DaHause show error when not able to connect.
            if(house.Connect(out ApiResult result) == false)
            {
                return result;
            }

            //Number of lamps
            int numberOfLamps = int.Parse(house.GetResponse("lamps"));

            // Create array for lamps
            Lamp[] Lamps = new Lamp[numberOfLamps];

            // Ask status foreach lamp
            for (int index = 0; index < Lamps.Length; index++)
            {
                bool Status = house.GetResponse("lamp " + index.ToString()).Contains("On");
                string Description = house.GetResponse("whereis lamp " + index);
                int Floor = int.Parse(Description.Split('@')[1].Split(' ')[1]);
                string Location = Description.Split('@')[1].Split(' ')[2];

                Lamps[index] = new Lamp { id = index + 1, on = Status, floor = Floor, location = Location };
            }

            //Close connection to the house.
            house.Close();
            return new ApiResult(Lamps);
        }

        //[HttpPut("")]
        //[MiddlewareFilter(typeof(TokenAuthorize))]


    }
}
