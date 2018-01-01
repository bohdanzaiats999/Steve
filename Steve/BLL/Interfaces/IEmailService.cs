using Steve.BLL.Models;
using Steve.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steve.BLL.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
        void TimerSendEmail();
        IList<EmailModel> GetEmailList();
        void SaveTimerData(int userId, EmailModel model);
        void CansellTask();
        void ChangePasswordByEmail(UserModel userModel);

    }
}
