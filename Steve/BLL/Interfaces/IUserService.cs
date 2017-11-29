using Steve.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Steve.BLL.Models;

namespace Steve.BLL.Interfaces
{
    public interface IUserService
    {
        void Create(UserModel userModel);
    }
}
