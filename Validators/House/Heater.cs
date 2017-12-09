using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators.House
{
    public class Heater
    {
        [Range(Domotica_API.Controllers.House.House.HEATER_MIN, Domotica_API.Controllers.House.House.HEATER_MAX)]
        public int temperature { get; set;  }
    }
}
