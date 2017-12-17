using System;
using System.Collections.Generic;
using System.Text;

namespace Steve.DAL.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public EmailEntity Email { get; set; }
    }
}
