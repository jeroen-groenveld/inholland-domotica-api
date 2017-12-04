using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class UserLogin
    {
        [MaxLength(60)]
        public string email { get; set; }

        [Required]
        [MaxLength(64), MinLength(8)]
        public string password { get; set; }
    }
}
