using Steve.BLL.Interfaces;
using Steve.DAL.Entities;
using Steve.DAL.Interfaces;
using Steve.DAL.Repositories;
using System.Collections.Generic;
using AutoMapper;
using Steve.BLL.Models;
using System;

namespace Steve.BLL.Services
{
    public class GoodsService : IGoodsService
    {
        IUnitOfWork Database { get; set; }
        public GoodsService()
        {
            Database = new UnitOfWork();
        }
        public IList<LaptopModel> GetLaptopList()
        {
            try
            {
                Mapper.Reset();
                Mapper.Initialize(m => m.CreateMap<LaptopEntity, LaptopModel>());
                var laptops = Mapper.Map<IList<LaptopEntity>, IList<LaptopModel>>(Database.Repository<LaptopEntity>().GetAllLaptops());

                return laptops;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Insert()
        {
            Database.Repository<LaptopEntity>().Insert(new LaptopEntity
            {
                Name = "HP ProBook 440 G3",
                Category = "Laptop",
                Color = "Black",
                Price = 17999
            });
            Database.SaveChanges();

            Database.Repository<LaptopEntity>().Insert(new LaptopEntity
            {
                Name = "Dell Inspiron 7577",
                Category = "Laptop",
                Color = "Black",
                Price = 46999
            });
            Database.SaveChanges();

            Database.Repository<LaptopEntity>().Insert(new LaptopEntity
            {
                Name = "Apple MacBook Pro Retina 15",
                Category = "Laptop",
                Color = "White",
                Price = 65399
            });
            Database.SaveChanges();
        }
    }
}
