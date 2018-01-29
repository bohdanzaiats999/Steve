using System;
using System.Collections.Generic;
using System.Text;

namespace Steve.BLL.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public LaptopModel Laptop { get; set; }
        public int Quantity { get; set; }
    }
}
