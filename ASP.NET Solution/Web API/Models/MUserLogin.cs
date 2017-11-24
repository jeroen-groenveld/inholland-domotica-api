using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_API.Models
{
    public class MUserLogin
    {
        [Required]
        [MaxLength(50)]
        public string email { get; set; }

        [Required]
        [MaxLength(64)]
        public string password { get; set; }
    }
}
