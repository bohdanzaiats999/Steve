using Microsoft.AspNetCore.Mvc;
using Steve.BLL.Interfaces;
using AutoMapper;
using Steve.BLL.Models;
using Steve.Web.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Steve.Web.Controllers
{
    public class GoodsController : Controller
    {
        IGoodsService goodsService;
        public int pageSize = 3;
        public GoodsController(IGoodsService goodsService)
        {
            this.goodsService = goodsService;
        }
        public IActionResult GoodsList(string category, int page = 1)
        {
            try
            {
                var laptopListViewModel = new LaptopListViewModel
                {
                    Laptops = goodsService.GetLaptopList()
                    .Where(p => category == null || p.Color == category)
                    .OrderBy(laptop => laptop.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList(),

                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = goodsService.GetLaptopList().Count
                    },
                    CurrentCategory = category
                };
                return View(laptopListViewModel);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
    }
}