using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Steve.Web.Models
{
    public class ChangePasswordByEmailViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string Login { get; set; }
    }
}
