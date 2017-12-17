using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Domotica_API.Models
{
    public class Move : Date.DateModelCreatedAt
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		public int game_id { get; set; }

		[IgnoreDataMember]
		[ForeignKey("game_id")]
		public virtual Game game { get; set; }

	    public int user_id { get; set; }

	    [IgnoreDataMember]
	    [ForeignKey("user_id")]
	    public virtual User user { get; set; }

	    [Range(0, 8)]
		public int position { get; set; }

        [Range(0, 8)]
        public int move_count { get; set; }
    }
}
