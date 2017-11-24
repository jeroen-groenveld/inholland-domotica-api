using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;
using Web_API.Models.TokenAuth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_API.Models.TokenAuth
{
    [Table("auth_refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }


        public int access_token_id { get; set; }

        [ForeignKey("access_token_id")]
        public virtual AccessToken access_token { get; set; }

        [DataType(DataType.Date)]
        public DateTime expires_at { get; set; }
    }
}
