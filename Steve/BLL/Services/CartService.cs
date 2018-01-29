using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steve.BLL.Services
{
    public class CartService : ICartService
    {
        private List<CartModel> cartCollection = new List<CartModel>();

        public void AddItem(LaptopModel laptop, int quantity)
        {
            CartModel cart = cartCollection
                .Where(g => g.Laptop.Id == laptop.Id)
                .FirstOrDefault();

            if (cart == null)
            {
                cartCollection.Add(new CartModel
                {
                    Laptop = laptop,
                    Quantity = quantity
                });
            }
            else
            {
                cart.Quantity += quantity;
            }
        }
        public void RemoveCart(LaptopModel game)
        {
            cartCollection.RemoveAll(l => l.Laptop.Id == game.Id);
        }
        public decimal ComputeTotalValue()
        {
            return cartCollection.Sum(e => e.Laptop.Price * e.Quantity);
        }
        public void Clear()
        {
            cartCollection.Clear();
        }
        public IList<CartModel> GetCartLine()
        {
             return cartCollection; 
        }
    }
}
