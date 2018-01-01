using MailKit.Net.Smtp;
using MimeKit;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using Steve.BLL.Security;
using Steve.DAL.Entities;
using Steve.DAL.Interfaces;
using Steve.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Steve.BLL.Services
{
    public class EmailService : IEmailService
    {
        IUnitOfWork Database { get; set; }
        public EmailService()
        {
            Database = new UnitOfWork();
        }
        private CancellationTokenSource CancellationToken { get; set; }

        // Cansell all tasks
        public void CansellTask()
        {
            if (CancellationToken != null)
            {
                CancellationToken.Cancel();
            }
        }
        public IList<EmailModel> GetEmailList()
        {
            Mapper.Initialize(m => m.CreateMap<EmailEntity, EmailModel>());
            var emails = Mapper.Map<IList<EmailEntity>, IList<EmailModel>>(Database.Repository<EmailEntity>().GetAllEmails());
            Mapper.Reset();
            return emails;
        }

        // Send Email using timer
        public async void TimerSendEmail()
        {
            CancellationToken = new CancellationTokenSource();

            // Get all email objects
            var emailModelList = GetEmailList();

            foreach (var emailModel in emailModelList)
            {

                DateTime sendingTime = (DateTime)emailModel.SendingTime;
                // Get intervar for timer
                TimeSpan interval = sendingTime - DateTime.Now;
                // When there is some interval
                if (interval > TimeSpan.Zero)
                {
                    await Task.Run(() =>
                    {
                        Thread.Sleep(interval);
                        emailModel.SendingTime = DateTime.MinValue;
                        SendEmail(emailModel);

                        Mapper.Initialize(m => m.CreateMap<EmailModel, EmailEntity>());
                        var emailEntity = Mapper.Map<EmailModel, EmailEntity>(emailModel);
                        Mapper.Reset();

                        Database.Repository<EmailEntity>().Update(emailEntity);
                        Database.SaveChanges();
                    });
                }
            }
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

            Mapper.Initialize(m => m.CreateMap<EmailModel,EmailEntity>());
            Mapper.Map<EmailModel, EmailEntity>(model, email);
            Mapper.Reset();

            Database.Repository<EmailEntity>().Update(email);
            Database.SaveChanges();
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

    }
}
