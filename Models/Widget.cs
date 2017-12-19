using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domotica_API.Models;
using Domotica_API.Models.TokenAuth;
using System.Drawing;

namespace Domotica_API.Models
{
    [Table("site_widgets")]
    public class Widget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(60)]
        public string name { get; set; }

        [MaxLength(350)]
        public string description { get; set; }

        [MaxLength(60)]
        public string component_name { get; set; }
    }
}
