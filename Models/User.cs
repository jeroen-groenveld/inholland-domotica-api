using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Domotica_API.Models.TokenAuth;
using Domotica_API.Models;

namespace Domotica_API.Models
{
    [Table("users")]
    public class User : Date.DateModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(60)]
        public string email { get; set; }

        [MaxLength(60)]
        public string name { get; set; }

        public int background_id { get; set;}

        [IgnoreDataMember]
        [ForeignKey("background_id")]
        public virtual Background background { get; set; }

        [IgnoreDataMember]
        //The total length of hashed password in bits is PASSWORD_HASH_SIZE + PASSWORD_HASH_SALT_SIZE.
        //base64 converts the bytes to characters, characters are 6 bits.
        //So the length of the base64 string is the total hashed password in bit divided by 6.
        //Example:
        // - PASSWORD_HASH_SIZE =       512 bits
        // - PASSWORD_HASH_SALT_SIZE =  256 bits
        // total length of the password in bits = 512 + 256 = 768.
        // total length of the base64 string = 768 / 6 = 128.
        // In this case the password column should be 128 chars long.
        [MaxLength((Config.Hash.PASSWORD_HASH_SIZE + Config.Hash.PASSWORD_HASH_SALT_SIZE) / 6)]
        public string password { get; set; }

        public bool is_admin { get; set; }

        [IgnoreDataMember]
        public List<AccessToken> Tokens { get; set; } = new List<AccessToken>();

        [IgnoreDataMember]
        public List<ActiveWidget> ActiveWidgets { get; set; } = new List<ActiveWidget>();

        [IgnoreDataMember]
        public List<Move> Moves { get; set; } = new List<Move>();

		[IgnoreDataMember]
        public List<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

        [IgnoreDataMember]
        public virtual List<Game> GamesPlayer1 { get; set; } = new List<Game>();

        [IgnoreDataMember]
        public virtual List<Game> GamesPlayer2 { get; set; } = new List<Game>();

        [IgnoreDataMember]
        public virtual List<Game> GamesWon { get; set; } = new List<Game>();
    }
}
