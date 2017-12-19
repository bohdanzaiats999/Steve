using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steve.Web.Models;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using Microsoft.IdentityModel.Protocols;

namespace Steve.Web.Controllers
{
    public class HomeController : Controller
    {
        IUserService userService;
        public HomeController(IUserService userService)
        {
            this.userService = userService;
            userService.TimerSendEmail();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegisterViewModel model)
        {
            try
            {
                userService.Registration(new UserModel
                {
                    Login = model.Login,
                    Password = model.Password,
                    Email = model.Email,
                    RoleId = (int)UserRoles.User
                });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return Content("Successful");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                userService.Login(new UserModel
                {
                    Login = model.Login,
                    Password = model.Password,
                });

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return userService.GetIdRole() == 1 ? RedirectToAction("Index", "AdminPanel") : RedirectToAction("Index", "UserPanel");
        }

        [HttpGet]
        public IActionResult ChangePasswordByEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePasswordByEmail(ChangePasswordByEmailViewModel model)
        {
            try
            {
                userService.ChangePasswordByEmail(new UserModel
                {
                    Login = model.Login
                });

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return Content("Your new password sent by email");
        }
    }
}
