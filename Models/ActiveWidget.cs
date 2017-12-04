using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domotica_API.Models;
using Domotica_API.Models.TokenAuth;

namespace Domotica_API.Models
{
    [Table("user_activewidgets")]
    public class ActiveWidget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User user { get; set; }

        public int widget_id { get; set; }

        [ForeignKey("widget_id")]
        public virtual Widget widget { get; set; }

        public string position { get; set; }
    }
}
