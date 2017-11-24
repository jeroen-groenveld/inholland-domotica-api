using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Models.House
{
    public class Item
    {
        public int id { get; set; }
        public string type { get; set; }
        public bool status { get; set; }
        public int floor { get; set; }
        public string location { get; set; }
    }
}
