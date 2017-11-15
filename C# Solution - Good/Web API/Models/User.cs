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
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(60)]
        public string email { get; set; }

        [MaxLength(60)]
        public string name { get; set; }

        [MaxLength(88)]
        public string password { get; set; }

        public List<AccessToken> Tokens { get; set; } = new List<AccessToken>();

        [DataType(DataType.Date)]
        public DateTime created_at { get; set; }

        [DataType(DataType.Date)]
        public DateTime updated_at { get; set; }
    }
}
