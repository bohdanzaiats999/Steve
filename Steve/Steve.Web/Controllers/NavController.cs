using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steve.BLL.Interfaces;
using Steve.BLL.Services;

namespace Steve.Web.Controllers
{
    [ViewComponent]
    public class NavController : Controller
    {
        IGoodsService goodsService;
        public NavController(IGoodsService goodsService)
        {
            this.goodsService = goodsService;
        }
        public IEnumerable<string> Menu()
        {
            IEnumerable<string> colors = goodsService.GetLaptopList()
             .Select(laptop => laptop.Color)
             .Distinct()
             .OrderBy(x => x);
            
            return colors;
        }


    }
}