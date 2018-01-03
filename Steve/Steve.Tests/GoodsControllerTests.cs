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
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            var mock = new Mock<IGoodsService>();
            mock.Setup(m => m.GetLaptopList()).Returns(GetLaptopList());

            GoodsController controller = new GoodsController(mock.Object);
            controller.pageSize = 3;

            // Act
            var result = controller.GoodsList(null, 2);
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
            var result = controller.GoodsList(null, 2);
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
            //HtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //// Утверждение
            //Assert.Equal(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
            //    + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            //    + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
            //    result.ToString());
        }

        [Fact]
        public void Can_Filter_Games()
        {
            // Arrange
            var mock = new Mock<IGoodsService>();
            mock.Setup(m => m.GetLaptopList()).Returns(GetLaptopList());

            var controller = new GoodsController(mock.Object);
            controller.pageSize = 3;

            // Action
            var result = controller.GoodsList("Black", 1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<LaptopListViewModel>(viewResult.Model).Laptops.ToList();

            // Assert
            Assert.Equal(3, model.Count());
            Assert.True(model[0].Name == "iPhone 7" && model[0].Color == "Black");
            Assert.True(model[1].Name == "iPhone 8" && model[1].Color == "Black");
        }

        [Fact]
        public void Can_Create_Categories()
        {
            // Arrange
            var mock = new Mock<IGoodsService>();
            mock.Setup(m => m.GetLaptopList()).Returns(GetLaptopList());

            NavController target = new NavController(mock.Object);

            // Act
            List<string> results = ((IEnumerable<string>)target.Invoke().Model).ToList();

            // Утверждение
            Assert.Equal(3, results.Count());
            Assert.Equal("Black", results[0]);
            Assert.Equal("Gray", results[1]);
            Assert.Equal("White", results[2]);
        }
        private IList<LaptopModel> GetLaptopList()
        {
            IList<LaptopModel> phones = new List<LaptopModel>
            {
                new LaptopModel { Id=1, Name="iPhone 7", Price=900, Color = "Black"},
                new LaptopModel { Id=2, Name="Meizu 6 Pro", Price=300, Color = "Gray"},
                new LaptopModel { Id=3, Name="Mi 5S", Price=400, Color = "White"},
                new LaptopModel { Id=4, Name="iPhone 7", Price=900, Color = "Black"},
                new LaptopModel { Id=4, Name="iPhone 8", Price=15000, Color = "Black"}

            };
            return phones;
        }


    }
}
