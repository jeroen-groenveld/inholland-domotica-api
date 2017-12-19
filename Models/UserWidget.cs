using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Domotica_API.Models;
using Domotica_API.Models.TokenAuth;

namespace Domotica_API.Models
{
    [Table("user_widgets")]
    public class UserWidget
    {
        [IgnoreDataMember]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [IgnoreDataMember]
        public int user_id { get; set; }

        [IgnoreDataMember]
        [ForeignKey("user_id")]
        public virtual User User { get; set; }

        public int widget_id { get; set; }

        [IgnoreDataMember]
        [ForeignKey("widget_id")]
        public virtual Widget Widget { get; set; }

        [MaxLength(1), MinLength(1)]
        public string column { get; set; }

        public int column_index { get; set; }
    }
}
