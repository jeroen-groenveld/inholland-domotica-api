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

        public DateTime? finished_at { get; set; }

        [IgnoreDataMember]
        public List<Score> Scores { get; set; } = new List<Score>();
    }
}
