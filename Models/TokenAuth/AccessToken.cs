using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Models;
using Domotica_API.Models.TokenAuth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Domotica_API.Models.TokenAuth
{
    [Table("auth_access_tokens")]
    public class AccessToken : Date.DateModelCreatedAt
    {
        [IgnoreDataMember]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int user_id { get; set; }

        [IgnoreDataMember]
        [ForeignKey("user_id")]
        public virtual User user { get; set; }

        public RefreshToken refresh_token { get; set; }

        [MaxLength(88)]
        public string token { get; set; }

        public DateTime expires_at { get; set; }
    }
}
