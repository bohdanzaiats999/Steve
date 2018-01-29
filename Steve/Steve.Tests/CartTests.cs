using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Steve.BLL.Models;
using Steve.BLL.Interfaces;
using Moq;
using Steve.BLL.Services;

namespace Steve.Tests
{

    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            // Arrange
            LaptopModel laptop1 = new LaptopModel { Id = 1, Name = "Laptop1" };
            LaptopModel laptop2 = new LaptopModel { Id = 2, Name = "Laptop2" };

            var cart = new CartService();

            // Act
            cart.AddItem(laptop1, 1);
            cart.AddItem(laptop2, 1);

            var results = cart.GetCartLine();

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(1, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange
            LaptopModel laptop1 = new LaptopModel { Id = 1, Name = "Laptop1" };
            LaptopModel laptop2 = new LaptopModel { Id = 2, Name = "Laptop2" };
            var cart = new CartService();

            //Act
            cart.AddItem(laptop1, 1);
            cart.AddItem(laptop2, 1);
            cart.AddItem(laptop1, 5);
            var result = cart.GetCartLine().OrderBy(c => c.Laptop.Id).ToList();

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(6, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            // Arrange
            LaptopModel laptop1 = new LaptopModel { Id = 1, Name = "Laptop1" };
            LaptopModel laptop2 = new LaptopModel { Id = 2, Name = "Laptop2" };
            LaptopModel laptop3 = new LaptopModel { Id = 3, Name = "Laptop3" };

            var cart = new CartService();

            // Act
            cart.AddItem(laptop1, 1);
            cart.AddItem(laptop2, 4);
            cart.AddItem(laptop3, 2);
            cart.AddItem(laptop2, 1);

            cart.RemoveCart(laptop2);

            //Assert
            Assert.Equal(0, cart.GetCartLine().Where(c => c.Laptop == laptop2).Count());
            Assert.Equal(2, cart.GetCartLine().Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            // Arrange
            LaptopModel laptop1 = new LaptopModel { Id = 1, Name = "Laptop1",Price = 1 };
            LaptopModel laptop2 = new LaptopModel { Id = 2, Name = "Laptop2" , Price = 1};

            var cart = new CartService();

            // Act
            cart.AddItem(laptop1, 3);
            cart.AddItem(laptop2, 2);
            cart.AddItem(laptop1, 5);
            decimal result = cart.ComputeTotalValue();

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void Can_Clear_Content()
        {
            // Arrange
            LaptopModel laptop1 = new LaptopModel { Id = 1, Name = "Laptop1", Price = 100 };
            LaptopModel laptop2 = new LaptopModel { Id = 2, Name = "Laptop2", Price = 55 };

            var cart = new CartService();

            // Act
            cart.AddItem(laptop1, 1);
            cart.AddItem(laptop2, 1);
            cart.AddItem(laptop1, 5);
            cart.Clear();

            // Assert

            Assert.Equal(0, cart.GetCartLine().Count);
        }
        private IList<LaptopModel> GetLaptopList()
        {
            IList<LaptopModel> laptops = new List<LaptopModel>
            {
            new LaptopModel { Id = 1, Name = "Laptop1"},
            new LaptopModel { Id = 1, Name = "Laptop2"},
            };
            return laptops;
        }
    }
}
