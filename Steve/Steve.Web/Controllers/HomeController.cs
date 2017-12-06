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
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(string login,string password,string confirmPassword,string Email)
        {
            var registerView = new RegisterViewModel
            {
                Login = login,
                Password = password,
                ConfirmPassword = confirmPassword,
                Email = Email,
                Role = UserRoles.User
            };
            try
            {
                userService.Registration(new UserModel
                {
                    Login = registerView.Login,
                    Password = registerView.Password,
                    Email = registerView.Email,
                    RoleId = (int)registerView.Role
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
        public IActionResult Login(string login,string password)
        {
            var loginView = new LoginViewModel
            {
                Login = login,
                Password = password
            };
            try
            {
                userService.Login(new UserModel
                {
                    Login = loginView.Login,
                    Password = loginView.Password,
                });

            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
            return userService.GetIdRole() == 1 ? View("Views/AdminPanel/Index.cshtml") : View("UserPanel");
        }

        [HttpGet]
        public IActionResult ChangePasswordByEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePasswordByEmail(string login)
        {
            var changePassView = new ChangePasswordByEmailViewModel
            {
                Login = login
            };
            try
            {
                userService.ChangePasswordByEmail(new UserModel
                {
                    Login = changePassView.Login
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
