using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web_API.Models;
using Web_API.Models.TokenAuth;
using System.Drawing;

namespace Web_API.Models
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

        public int size_w { get; set; }

        public int size_h { get; set; }

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }

        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }
    }
}
