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
            var users = userService.GetAllUsers();
            List<UserModel> userList = new List<UserModel>();

            foreach (var user in users)
            {
                userList.Add(new UserModel
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Checked = false

                });
            }
            return View(userList);
        }
        [HttpPost]
        public IActionResult SendEmail(string fromAdressTitle, string toAddress, string subject, string bodyContent, List<UserModel> list)
        {
            try
            {
                if (toAddress != null)
                {
                    userService.SendEmail(fromAdressTitle, toAddress, subject, bodyContent);
                }

                foreach (var user in list)
                {
                    if (user.Checked == true)
                    {
                        userService.SendEmail(fromAdressTitle, user.Email, subject, bodyContent);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return View(list);
        }
    }
}