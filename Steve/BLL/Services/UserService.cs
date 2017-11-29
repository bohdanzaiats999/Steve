﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Steve.BLL.Infrastructure;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using Steve.DAL.Interfaces;
using Steve.DAL.Entities;
using Steve.DAL.Repositories;

namespace Steve.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService()
        {
            Database = new UnitOfWork();
        }
        public void Create(UserModel userModel)
        {
            UserEntity user = Database.Repository<UserEntity>().GetByLogin(userModel.Login);

            if (user != null)
            {
                throw new Exception("User already exist");
            }

            try
            {
                Database.Repository<UserEntity>().Insert(new UserEntity
                {
                    Login = userModel.Login,
                    Password = userModel.Password
                });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            Database.SaveChanges();
        }
    }
}