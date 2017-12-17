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
    [Table("games")]
    public class Game : Date.DateModelCreatedAt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user1_id { get; set; }
        public int? user2_id { get; set; }
        public int? user_winner_id { get; set; }

        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual User UserWinner { get; set; }

        public int status { get; set; }

	    public DateTime? finished_at { get; set; }

	    public List<Move> Moves { get; set; } = new List<Move>();
    }

	public class GameStatus
	{
		public const int waiting_join = 0;
		public const int waiting_invite = 1;
		public const int started = 2;
		public const int finished = 3;
	}
}
