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
    [Table("user_bookmarks")]
    public class Bookmark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User user { get; set; }

        [MaxLength(60)]
        public string name { get; set; }

        //Url max size 2083.
        [MaxLength(2083)]
        public string url { get; set; }

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }

        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }
    }
}
