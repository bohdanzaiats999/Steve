using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steve.BLL.Interfaces;
using System.Collections;
using Steve.Web.Models;
using Steve.BLL.Models;
using AutoMapper;

namespace Steve.Web.Controllers
{
    public class AdminPanelController : Controller
    {
        IUserService userService;
        IEmailService emailService;

        public AdminPanelController(IUserService userService, IEmailService emailService)
        {
            this.userService = userService;
            this.emailService = emailService;

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
                    Email = user.Email,
                    RoleId = user.RoleId,
                    Checked = false
                });
            }

            return View(new EmailViewModel { Users = userList });
        }
        [HttpPost]
        public IActionResult SendEmail(EmailViewModel viewModel)
        {
            try
            {
                if (viewModel.ToAddress != null)
                {
                    Mapper.Initialize(m => m.CreateMap<EmailViewModel, EmailModel>()
                    .ForMember(f => f.Users, opt => opt.Ignore()));
                    var model = Mapper.Map<EmailViewModel, EmailModel>(viewModel);
                    Mapper.Reset();

                    emailService.SendEmail(model);
                }
                foreach (var user in viewModel.Users)
                {
                    if (user.Checked == true)
                    {
                        Mapper.Initialize(m => m.CreateMap<EmailViewModel, EmailModel>()
                        .ForMember(f => f.Users, opt => opt.Ignore()));
                        var email = Mapper.Map<EmailViewModel, EmailModel>(viewModel);
                        Mapper.Reset();

                        email.ToAddress = user.Email;

                        if (viewModel.SendingTime <= DateTime.Now)
                        {
                            emailService.SendEmail(email);
                        }
                        else
                        {
                            emailService.SaveTimerData(user.Id, email);
                            emailService.CansellTask();
                            emailService.TimerSendEmail();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return View(viewModel);
        }
    }
}