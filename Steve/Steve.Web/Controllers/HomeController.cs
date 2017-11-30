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


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(string Email,string Password,string ConfirmPassword)
        {
            var userView = new UserViewModel
            {
                Login = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };
            try
            {
                userService.Create(new UserModel
                {
                    Login = userView.Login,
                    Password = userView.Password,
                });
            }
            catch (Exception ex)
            {
               return Content(ex.Message);
            }
            return Content("Successful");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
