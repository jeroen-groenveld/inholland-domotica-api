using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class RefreshToken
    {
        [Required]
        [MaxLength(88)]
        public string token { get; set; }
    }
}
