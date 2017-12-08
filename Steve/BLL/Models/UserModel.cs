using System;
using System.Collections.Generic;
using System.Text;

namespace Steve.BLL.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool Checked { get; set; }

    }
}
