using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_API.Models.TokenAuth
{
    public class MRefreshToken
    {
        [Required]
        [MaxLength(88)]
        public string token { get; set; }
    }
}
