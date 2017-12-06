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
            string FromAddress = "bohdan2131@gmail.com";
            string FromAdressTitle = "Email from Bohdan Zaiats";
            //To Address  
            string ToAddress = user.Email;
            string ToAdressTitle = user.Email;
            string Subject = "Generating new pasword";
            string BodyContent = $"Your New password: {newPassword}";

            //Smtp Server  
            string SmtpServer = "smtp.gmail.com";
            //Smtp Port Number  
            int SmtpPortNumber = 587;

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
            mimeMessage.Subject = Subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = BodyContent
            };

            using (var client = new SmtpClient())
            {

                client.Connect(SmtpServer, SmtpPortNumber, false);
                client.Authenticate("bohdan2131@gmail.com", "bohdan123");
                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }

        public int GetIdRole()
        {
            return IdRole;
        }

        public IQueryable GetAllUsers()
        {
            return Database.Repository<UserEntity>().Include(u => u.Login);
        }
    }
}
