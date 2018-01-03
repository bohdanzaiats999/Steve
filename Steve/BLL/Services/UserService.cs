using System;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using Steve.DAL.Interfaces;
using Steve.DAL.Entities;
using Steve.DAL.Repositories;
using Steve.BLL.Security;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;

namespace Steve.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        private static int IdRole { get; set; }

        // Cansell all Task

        public UserService()
        {
            Database = new UnitOfWork();
        }

        public void Registration(UserModel userModel)
        {
            UserEntity user = Database.Repository<UserEntity>().GetByLogin(userModel.Login);

            if (user != null)
            {
                throw new Exception("User already exist");
            }
            // Encrypt the password
            string encryptedPassword = new AesCrypt().EncryptAes(userModel.Password);

            try
            {
                Database.Repository<UserEntity>().Insert(new UserEntity
                {
                    Login = userModel.Login,
                    Password = encryptedPassword,
                    Email = new EmailEntity { EmailAdress = userModel.Email },
                    RoleId = userModel.RoleId
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            Database.SaveChanges();
            IdRole = userModel.RoleId;
        }

        public void Login(UserModel userModel)
        {
            UserEntity user = Database.Repository<UserEntity>().GetByLogin(userModel.Login);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            string encryptedPassword = new AesCrypt().EncryptAes(userModel.Password);
            // Compare password in database and user paswword
            if (user.Password != encryptedPassword)
            {
                throw new Exception("Password is wrong");
            }
            IdRole = user.RoleId;
        }

        // Get user role
        public int GetIdRole()
        {
            return IdRole;
        }

        // Get all users with email objects
        public IList<UserModel> GetAllUsersWithEmail()
        {
            Mapper.Reset();
            Mapper.Initialize(m => m.CreateMap<UserEntity, UserModel>()
            .ForMember("Email",f=>f.MapFrom(x=>x.Email.EmailAdress)));
            var users = Mapper.Map<IList<UserEntity>, IList<UserModel>>(Database.Repository<UserEntity>().Include(e => e.Email).ToList());

            return users;
        }

    }
}
