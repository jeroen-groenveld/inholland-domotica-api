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
    [Table("games")]
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }

        [DataType(DataType.Date)]
        public DateTime finished_at { get; set; }
    }
}
