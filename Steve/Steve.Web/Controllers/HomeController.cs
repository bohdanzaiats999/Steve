using System;
using Microsoft.AspNetCore.Mvc;
using Steve.Web.Models;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using AutoMapper;

namespace Steve.Web.Controllers
{
    public class HomeController : Controller
    {
        IUserService userService;
        IEmailService emailService;
        IGoodsService goodsService;

        public HomeController(IUserService userService, IEmailService emailService, IGoodsService goodsService)
        {
            this.userService = userService;
            this.emailService = emailService;
            this.goodsService = goodsService;
            emailService.TimerSendEmail();
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(goodsService.GetLaptopList());
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
                Mapper.Reset();
                Mapper.Initialize(m => m.CreateMap<RegisterViewModel, UserModel>());
                var user = Mapper.Map<RegisterViewModel, UserModel>(model);

                user.RoleId = (int)UserRoles.User;
                userService.Registration(user);
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
        public IActionResult Login(LoginViewModel viewModel)
        {
            try
            {
                Mapper.Reset();
                Mapper.Initialize(m => m.CreateMap<LoginViewModel, UserModel>());
                var model = Mapper.Map<LoginViewModel, UserModel>(viewModel);

                userService.Login(model);
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
                emailService.ChangePasswordByEmail(new UserModel
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
