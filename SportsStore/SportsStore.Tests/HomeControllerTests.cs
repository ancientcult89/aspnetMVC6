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
            ProductListViewModel result = controller.Index()?.ViewData.Model as ProductListViewModel ?? new();

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
            ProductListViewModel result = controller.Index(2)?.ViewData.Model as ProductListViewModel ?? new();

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
            ProductListViewModel result = controller.Index(2)?.ViewData.Model as ProductListViewModel ?? new();

            //assert
            PageInfo pageInfo = result.PageInfo;
            Assert.Equal(currentPageMock, pageInfo.CurrentPage);
            Assert.Equal(pageSizeMock, pageInfo.ItemsPerPage);
            Assert.Equal(totalItemsMock, pageInfo.TotalItems);
            Assert.Equal(totalPagesMock, pageInfo.TotalPages);
        }

    }
}
