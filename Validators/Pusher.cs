using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class PusherUser
    {
        [Required]
        public string socket_id { get; set; }
    }

    public class PusherUserGame
    {
        [Required]
        public string socket_id { get; set; }

        [Required]
        public int game_id { get; set; }
    }
}
