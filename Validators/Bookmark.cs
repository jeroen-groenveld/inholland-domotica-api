using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class Bookmark
    {
        [Required]
        [MaxLength(60)]
        public string name { get; set; }

        //Url max size 2083.
        [Required]
        [MaxLength(2083)]
        public string url { get; set; }
    }
}
