using Steve.BLL.Infrastructure;
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
        void ChangePasswordByEmail(UserModel userModel);
        int GetIdRole();
        void SendEmail(EmailModel emailModel);
        void GetAndSendInTime();
        IList<UserEntity> GetAllUsersWithEmail();
        IList<EmailEntity> GetEmailList();
        void SaveTimerData(int userId, EmailModel model);
        void CansellTask();
    }
}
