using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web_API.Models;
using Web_API.Models.TokenAuth;

namespace Web_API.Models
{
    [Table("site_backgrounds")]
    public class Background
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(60)]
        public string name { get; set; }

        [MaxLength(350)]
        public string description { get; set; }

        public string data { get; set; }
    }
}
