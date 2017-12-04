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
    [Table("user_bookmarks")]
    public class Bookmark : Date.DateModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        [IgnoreDataMember]
        [ForeignKey("user_id")]
        public virtual User user { get; set; }

        [MaxLength(60)]
        public string name { get; set; }

        //Url max size 2083.
        [MaxLength(2083)]
        public string url { get; set; }
    }
}
