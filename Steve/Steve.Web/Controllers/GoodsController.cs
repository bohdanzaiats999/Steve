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
        public IActionResult GoodsList(int page = 1)
        {
            try
            {
                var laptopListViewModel = new LaptopListViewModel
                {
                    Laptops = goodsService.GetLaptopList().
                    OrderBy(laptop => laptop.Price)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList(),

                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = goodsService.GetLaptopList().Count
                    }

                };
                return View(laptopListViewModel);
                

                //var laptopsModel = goodsService.GetLaptopList();

                //Mapper.Initialize(m => m.CreateMap<LaptopModel, LaptopViewModel>());
                //var laptopsView = Mapper.Map<IList<LaptopModel>, IList<LaptopViewModel>>(laptopsModel);
                //Mapper.Reset();

                //return View(laptopsView
                //    .OrderBy(laptop => laptop.Price)
                //    .Skip((page - 1) * pageSize)
                //    .Take(pageSize).ToList());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
    }
}