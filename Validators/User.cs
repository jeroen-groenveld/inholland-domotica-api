using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class User
    { 
        public class UserRegister
        {
            [Required]
            [MaxLength(60)]
            public string email { get; set; }

            [Required]
            [MaxLength(60)]
            public string name { get; set; }

            [Required]
            [MaxLength(64), MinLength(8)]
            public string password { get; set; }
        }

        public class UserLogin
        {
            [Required]
            [MaxLength(60)]
            public string email { get; set; }

            [Required]
            [MaxLength(64), MinLength(8)]
            public string password { get; set; }
        }

        public class UserProfile
        {
            [Required]
            [MaxLength(60)]
            public string name { get; set; }

            [Required]
            public int background_id { get; set; }
        }
    }
}
