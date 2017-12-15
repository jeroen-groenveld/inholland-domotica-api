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

	    public int user_1_id { get; set; }

	    public int? user_2_id { get; set; }

	    public int? user_winner_id { get; set; }

	    [IgnoreDataMember]
	    [ForeignKey("user_1_id")]
	    public virtual User user_1 { get; set; }

	    [IgnoreDataMember]
	    [ForeignKey("user__2_id")]
	    public virtual User user_2 { get; set; }

	    [IgnoreDataMember]
	    [ForeignKey("user_winner_id")]
	    public virtual User user_winner { get; set; }

		public int status { get; set; }

	    public DateTime? finished_at { get; set; }

	    public List<Move> Moves { get; set; }
    }

	public class GameStatus
	{
		public const int waiting_join = 0;
		public const int waiting_invite = 1;
		public const int started = 2;
		public const int completed = 3;
	}
}
