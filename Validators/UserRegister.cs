using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class UserRegister : UserLogin
    {
        [Required]
        [MaxLength(60)]
        public string name { get; set; }
    }
}