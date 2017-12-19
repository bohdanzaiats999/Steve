using System;
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
using System.Threading.Tasks;
using System.Threading;

namespace Steve.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        private static int IdRole { get; set; }

        // Cansell all Task
        private CancellationTokenSource CancellationToken { get; set; }

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
        // Change password if user forgot it and send it on Email
        public void ChangePasswordByEmail(UserModel userModel)
        {
            string newPassword = string.Empty;
            string encryptedNewPassword = string.Empty;

            UserEntity user = Database.Repository<UserEntity>().GetByLogin(userModel.Login);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            // Generate uniqe password
            do
            {
                newPassword = new GenerateNewPassword().Generate();
                encryptedNewPassword = new AesCrypt().EncryptAes(newPassword);
            }
            while (newPassword == user.Password);


            user.Password = encryptedNewPassword;
            Database.Repository<UserEntity>().Update(user);
            Database.SaveChanges();

            // Email data
            string fromAdressTitle = "Email from Bohdan Zaiats";
            string toAddress = user.Email.EmailAdress;
            string subject = "Generating new pasword";
            string bodyContent = $"Your New password: {newPassword}";

            SendEmail(new EmailModel
            {
                FromAdressTitle = fromAdressTitle,
                ToAddress = toAddress,
                Subject = subject,
                BodyContent = bodyContent
            });
        }

        public void SendEmail(EmailModel emailModel)
        {

            string fromAddress = "bohdan2131@gmail.com";
            string smtpServer = "smtp.gmail.com";
            int smtpPortNumber = 587;

            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(emailModel.FromAdressTitle, fromAddress));
                mimeMessage.To.Add(new MailboxAddress(emailModel.ToAddress, emailModel.ToAddress));
                mimeMessage.Subject = emailModel.Subject;
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = emailModel.BodyContent
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
        // Save Email data in database if using timer
        public void SaveTimerData(int userId, EmailModel model)
        {
            var email = Database.Repository<EmailEntity>().GetById(userId);

            email.BodyContent = model.BodyContent;
            email.Subject = model.Subject;
            email.ToAddress = model.ToAddress;
            email.FromAdressTitle = model.FromAdressTitle;
            email.SendingTime = model.SendingTime;

            Database.Repository<EmailEntity>().Update(email);
            Database.SaveChanges();
        }
        // Cansell all tasks
        public void CansellTask()
        {
            if (CancellationToken != null)
            {
                CancellationToken.Cancel();
            }
        }
        // Send Email using timer
        public async void TimerSendEmail()
        {
            CancellationToken = new CancellationTokenSource();

            // Get all email objects
            var emailList = GetEmailList();

            foreach (var email in emailList)
            {
                
                if (email.SendingTime != null)
                {
                    DateTime sendingTime = (DateTime)email.SendingTime;
                    // Get intervar for timer
                    TimeSpan interval = sendingTime - DateTime.Now;
                    // When there is some interval
                    if (interval > TimeSpan.Zero)
                    {
                        await Task.Run(() =>
                        {
                            Thread.Sleep(interval);
                            SendEmail(new EmailModel
                            {
                                FromAdressTitle = email.FromAdressTitle,
                                ToAddress = email.ToAddress,
                                Subject = email.Subject,
                                BodyContent = email.BodyContent
                            });
                            email.SendingTime = DateTime.MinValue;
                            Database.Repository<EmailEntity>().Update(email);
                            Database.SaveChanges();
                        });
                    }
                }
            }
        }
        // Get user role
        public int GetIdRole()
        {
            return IdRole;
        }
        // Get all users with email objects
        public IList<UserEntity> GetAllUsersWithEmail()
        {
            return Database.Repository<UserEntity>().Include(e => e.Email).ToList();
        }
        // Get email objects
        public IList<EmailEntity> GetEmailList()
        {
            return Database.Repository<EmailEntity>().GetAllEmails();
        }
    }
}
