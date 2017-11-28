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
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(60)]
        public string email { get; set; }

        [MaxLength(60)]
        public string name { get; set; }

        public int background_id { get; set;}

        [ForeignKey("background_id")]
        public virtual Background background { get; set; }
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

        public List<AccessToken> Tokens { get; set; } = new List<AccessToken>();

        public List<ActiveWidget> ActiveWidgets { get; set; } = new List<ActiveWidget>();

        public List<Score> Scores { get; set; } = new List<Score>();

        public List<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }

        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }
    }
}
