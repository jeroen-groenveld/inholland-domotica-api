using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Validators
{
    public class UserWidget
    {
        [Required]
        [MaxLength(1), MinLength(1)]
        [RegularExpression("[A|B|C|D]")]
        public string column { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int column_index { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int widget_id { get; set; }
    }

    public class UserWidgetList
    {
        [Required]
        public List<UserWidget> user_widgets { get; set; }
    }
}
