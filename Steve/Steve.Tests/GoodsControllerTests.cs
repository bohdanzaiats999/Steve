using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Steve.BLL.Interfaces;
using Steve.BLL.Models;
using Steve.BLL.Services;
using Steve.Web.Controllers;
using Steve.Web.HtmlHelpers;
using Steve.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Steve.Tests
{
    public class GoodsControllerTests
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfPhones()
        {
            // Arrange
            var mock = new Mock<IGoodsService>();
            mock.Setup(m => m.GetLaptopList()).Returns(GetLaptopList());
            var controller = new GoodsController(mock.Object);
            controller.pageSize = 5;

            // Act
            var result = controller.GoodsList();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IList<LaptopViewModel>>(viewResult.Model);
            Assert.Equal(GetLaptopList().Count, model.Count());

            Assert.True(model.Count == 5);
            Assert.Equal(300, model[0].Price);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            var mock = new Mock<IGoodsService>();
            mock.Setup(m => m.GetLaptopList()).Returns(GetLaptopList());

            GoodsController controller = new GoodsController(mock.Object);
            controller.pageSize = 3;

            // Act
            var result = controller.GoodsList(2);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<LaptopListViewModel>(viewResult.Model);

            // Assert
            PagingInfo pageInfo = model.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            var mock = new Mock<IGoodsService>();
            mock.Setup(m => m.GetLaptopList()).Returns(GetLaptopList());

            var controller = new GoodsController(mock.Object);
            controller.pageSize = 3;

            // Act
            var result = controller.GoodsList(2);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<LaptopListViewModel>(viewResult.Model);

            // Assert
            List<LaptopModel> laptops = model.Laptops.ToList();
            Assert.True(laptops.Count == 2);
            Assert.Equal("iPhone 7", laptops[0].Name);
            Assert.Equal("iPhone 8", laptops[1].Name);
        }

        [Fact]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            HtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.Equal(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        private IList<LaptopModel> GetLaptopList()
        {
            IList<LaptopModel> phones = new List<LaptopModel>
            {
                new LaptopModel { Id=1, Name="iPhone 7",  Price=900},
                new LaptopModel { Id=2, Name="Meizu 6 Pro",  Price=300},
                new LaptopModel { Id=3, Name="Mi 5S", Price=400},
                new LaptopModel { Id=4, Name="iPhone 7", Price=900},
                new LaptopModel { Id=4, Name="iPhone 8", Price=15000}

            };
            return phones;
        }


    }
}
