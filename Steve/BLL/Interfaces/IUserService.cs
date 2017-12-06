﻿using Steve.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Steve.BLL.Models;
using System.Collections;
using System.Linq;

namespace Steve.BLL.Interfaces
{
    public interface IUserService
    {
        void Registration(UserModel userModel);
        void Login(UserModel userModel);
        void ChangePasswordByEmail(UserModel userModel);
        int GetIdRole();
        IQueryable GetAllUsers();



    }
}
