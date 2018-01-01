using Steve.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Steve.Web.Models
{
    public class EmailViewModel
    {

        [Required]
        [Display(Name = "From Adress Title")]
        public string FromAdressTitle { get; set; }
        [Required]
        [Display(Name = "To Address")]
        public string ToAddress { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Body Content")]
        public string BodyContent { get; set; }
        public DateTime SendingTime { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
