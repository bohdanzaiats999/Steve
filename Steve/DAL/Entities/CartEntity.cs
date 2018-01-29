using System;
using System.Collections.Generic;
using System.Text;

namespace Steve.DAL.Entities
{
    class CartEntity
    {
        public LaptopEntity Laptop { get; set; }
        public int Quantity { get; set; }
    }
}
