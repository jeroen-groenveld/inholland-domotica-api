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
    [Table("auth_refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int access_token_id { get; set; }

        [IgnoreDataMember]
        [ForeignKey("access_token_id")]
        public virtual AccessToken access_token { get; set; }
    }
}
