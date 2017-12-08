using System;
using Steve.BLL.Infrastructure;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using Steve.DAL.Interfaces;
using Steve.DAL.Entities;
using Steve.DAL.Repositories;
using Steve.BLL.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Steve.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }
        private static int IdRole { get; set; }
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

            string encryptedPassword = new AesCrypt().EncryptAes(userModel.Password);

            try
            {
                Database.Repository<UserEntity>().Insert(new UserEntity
                {
                    Login = userModel.Login,
                    Password = encryptedPassword,
                    Email = userModel.Email,
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

            if (user.Password != encryptedPassword)
            {
                throw new Exception("Password is wrong");
            }
            IdRole = user.RoleId;
        }

        public void ChangePasswordByEmail(UserModel userModel)
        {
            string newPassword = string.Empty;
            string encryptedNewPassword = string.Empty;

            UserEntity user = Database.Repository<UserEntity>().GetByLogin(userModel.Login);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            do
            {
                newPassword = new GenerateNewPassword().Generate();
                encryptedNewPassword = new AesCrypt().EncryptAes(newPassword);
            }
            while (newPassword == user.Password);


            user.Password = encryptedNewPassword;
            Database.Repository<UserEntity>().Update(user);
            Database.SaveChanges();

            //From Address  
            string fromAdressTitle = "Email from Bohdan Zaiats";
            //To Address  
            string toAddress = user.Email;
            string subject = "Generating new pasword";
            string bodyContent = $"Your New password: {newPassword}";

            SendEmail(fromAdressTitle,toAddress,subject,bodyContent);
        }

        public void SendEmail(string fromAdressTitle,string toAddress,string subject,string bodyContent)
        {
            string fromAddress = "bohdan2131@gmail.com";
            string smtpServer = "smtp.gmail.com";
            int smtpPortNumber = 587;

            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(fromAdressTitle, fromAddress));
                mimeMessage.To.Add(new MailboxAddress(toAddress, toAddress));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = bodyContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, smtpPortNumber, false);
                    client.Authenticate("bohdan2131@gmail.com", "bohdan123");
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int GetIdRole()
        {
            return IdRole;
        }

        public IList<UserEntity> GetAllUsers()
        {
            return Database.Repository<UserEntity>().GetAll();
        }
    }
}
