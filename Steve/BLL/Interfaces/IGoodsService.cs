using Steve.BLL.Models;
using Steve.DAL.Entities;
using System.Collections.Generic;

namespace Steve.BLL.Interfaces
{
    public interface IGoodsService
    {
        IList<LaptopModel> GetLaptopList();
        void Insert();
    }
}
