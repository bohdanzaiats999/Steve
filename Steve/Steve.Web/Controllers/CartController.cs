using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using Microsoft.AspNetCore.Session;

namespace Steve.Web.Controllers
{
    public class CartController : Controller
    {
        ICartService cartService;
        IGoodsService goodsService;
        public CartController(ICartService cartService, IGoodsService goodsService)
        {
            this.cartService = cartService;
            this.goodsService = goodsService;
        }

        //public RedirectToRouteResult AddToCart(int laptopId, string returnUrl)
        //{
        //    LaptopModel laptop = goodsService.GetLaptopList().FirstOrDefault(l => l.Id == laptopId);

        //    if (laptop != null)
        //    {

        //    }
        //}
        //public CartModel GetCart()
        //{
        //    CartModel cart = (CartModel)Microsoft.AspNetCore.Session.["Cart"];

        //    return cart;
        //}
    }
}