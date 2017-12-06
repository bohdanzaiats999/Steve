using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steve.BLL.Interfaces;
using System.Collections;

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
    }
}