using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Steve.BLL.Models;
using System.Collections;
using System.Linq;
using Steve.DAL.Entities;

namespace Steve.BLL.Interfaces
{
    public interface IUserService
    {
        void Registration(UserModel userModel);
        void Login(UserModel userModel);
        int GetIdRole();
        IList<UserModel> GetAllUsersWithEmail();

    }
}
