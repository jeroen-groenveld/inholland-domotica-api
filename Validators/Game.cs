using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class GameCreate
    {
        [MaxLength(60)]
        public string player2_email { get; set; }
    }

	public class GameJoin
	{
		[Required]
		public int id { get; set; }
	}

	public class Move
	{
		[Required]
		public int game_id { get; set; }

		[Required]
		[Range(0, 8)]
		public int position { get; set; }
	}
}
