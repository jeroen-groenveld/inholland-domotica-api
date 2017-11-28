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
    [Table("game_scores")]
    public class Score
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual User user { get; set; }


        public int game_id { get; set; }

        [ForeignKey("game_id")]
        public virtual Game game { get; set; }


        public int score { get; set; }
    }
}
