using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class GameCreate
    {
	    public int user_2_id { get; set; }
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
		[Range(1, 9)]
		public int position { get; set; }
	}
}
