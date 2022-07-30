using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SportsStore.Models;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        { 
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"}
                }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);

            //act
            ProductListViewModel result = controller.Index(null)?.ViewData.Model as ProductListViewModel ?? new();

            //assert
            Product[] products = result.Products.ToArray();
            Assert.True(products.Length == 2);
            Assert.Equal("P1", products[0].Name);
            Assert.Equal("P2", products[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
                }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);
            controller.pageSize = 3;

            //act
            ProductListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductListViewModel ?? new();

            //assert
            Product[] products = result.Products.ToArray();
            Assert.True(products.Length == 2);
            Assert.Equal("P4", products[0].Name);
            Assert.Equal("P5", products[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            }).AsQueryable<Product>());

            //arrange
            int pageSizeMock = 3;
            int currentPageMock = 2;
            int totalItemsMock = mock.Object.Products.Count();
            int totalPagesMock = (int)Math.Ceiling((decimal)totalItemsMock / pageSizeMock);
            HomeController controller = new HomeController(mock.Object) { pageSize = pageSizeMock };

            //act
            ProductListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductListViewModel ?? new();

            //assert
            PageInfo pageInfo = result.PageInfo;
            Assert.Equal(currentPageMock, pageInfo.CurrentPage);
            Assert.Equal(pageSizeMock, pageInfo.ItemsPerPage);
            Assert.Equal(totalItemsMock, pageInfo.TotalItems);
            Assert.Equal(totalPagesMock, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        { 
            //arrange
            //create the mock repository
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
                }).AsQueryable<Product>());

            //arrange - create a controller and make page size 3 items
            HomeController controller = new HomeController(mock.Object);
            controller.pageSize = 3;

            //action
            Product[] result = (controller.Index("Cat2", 1)?.ViewData.Model as ProductListViewModel ?? new()).Products.ToArray();

            //assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        { 
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable<Product>());

            HomeController target = new HomeController(mock.Object);
            target.pageSize = 3;

            Func<ViewResult, ProductListViewModel?> GetModel = result => result?.ViewData?.Model as ProductListViewModel;

            //action
            int? res1 = GetModel(target.Index("Cat1"))?.PageInfo.TotalItems;
            int? res2 = GetModel(target.Index("Cat2"))?.PageInfo.TotalItems;
            int? res3 = GetModel(target.Index("Cat3"))?.PageInfo.TotalItems;
            int? resAll = GetModel(target.Index(null))?.PageInfo.TotalItems;

            //assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
