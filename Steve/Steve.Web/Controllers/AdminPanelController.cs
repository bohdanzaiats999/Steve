using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steve.BLL.Interfaces;
using System.Collections;
using Steve.Web.Models;
using Steve.BLL.Models;

namespace Steve.Web.Controllers
{
    public class AdminPanelController : Controller
    {
        IUserService userService;

        public AdminPanelController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SendEmail()
        {
            var users = userService.GetAllUsersWithEmail();
            List<UserModel> userList = new List<UserModel>();

            foreach (var user in users)
            {
                userList.Add(new UserModel
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email.EmailAdress,
                    RoleId = user.RoleId,
                    Checked = false
                });
            }

            ViewBag.A = userList;
            return View(new EmailViewModel { UserList = userList });
        }
        [HttpPost]
        public IActionResult SendEmail(EmailViewModel model)
        {
            try
            {
                if (model.ToAddress != null)
                {
                    userService.SendEmail(new EmailModel
                    {
                        FromAdressTitle = model.FromAdressTitle,
                        ToAddress = model.ToAddress,
                        Subject = model.Subject,
                        BodyContent = model.BodyContent
                    });
                }
                foreach (var user in model.UserList)
                {
                    if (user.Checked == true)
                    {
                        var email = new EmailModel
                        {
                            FromAdressTitle = model.FromAdressTitle,
                            ToAddress = user.Email,
                            Subject = model.Subject,
                            BodyContent = model.BodyContent,
                            SendingTime = model.SendingTime
                        };

                        if (model.SendingTime <= DateTime.Now)
                            userService.SendEmail(email);
                        else
                        {
                            userService.SaveTimerData(user.Id, email);
                            userService.CansellTask();
                            userService.GetAndSendInTime();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return View(model);
        }
    }
}