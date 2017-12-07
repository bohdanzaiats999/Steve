using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steve.BLL.Interfaces;
using System.Collections;
using Steve.Web.Models;

namespace Steve.Web.Controllers
{
    public class AdminPanelController : Controller
    {
        IUserService userService;

        public AdminPanelController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Index()
        {
            return View(userService.GetAllUsers());
        }
        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendEmail(string fromAdressTitle, string toAddress, string subject, string bodyContent)
        {
            var emailView = new EmailViewModel
            {
                FromAdressTitle = fromAdressTitle,
                ToAddress = toAddress,
                Subject = subject,
                BodyContent = bodyContent
            };

            try
            {
                userService.SendEmail(fromAdressTitle, toAddress, subject, bodyContent);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return Content("Successful");
        }
    }
}