using Steve.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steve.BLL.Interfaces
{
    public interface ICartService
    {
        void AddItem(LaptopModel laptop, int quantity);
        void RemoveCart(LaptopModel game);
        decimal ComputeTotalValue();
        void Clear();
        IList<CartModel> GetCartLine();
    }
}
