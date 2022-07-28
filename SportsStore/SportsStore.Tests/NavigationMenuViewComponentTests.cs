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
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        { 
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
                {
                    new Product {ProductId = 1, Name = "P1",
                        Category = "Apples"},
                    new Product {ProductId = 2, Name = "P2",
                        Category = "Apples"},
                    new Product {ProductId = 3, Name = "P3",
                        Category = "Plums"},
                    new Product {ProductId = 4, Name = "P4",
                        Category = "Oranges"},
                }).AsQueryable<Product>());
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            //act = get the set of categories
            string [] result = ((IEnumerable<string>?)(target.Invoke() as ViewViewComponentResult)
                ?.ViewData?.Model ?? Enumerable.Empty<string>()).ToArray();

            //assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, result));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            //arrange
            string categoryToSelect = "Apples";
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
                {
                    new Product {ProductId = 1, Name = "P1",
                        Category = "Apples"},
                    new Product {ProductId = 4, Name = "P4",
                        Category = "Oranges"},
                }).AsQueryable<Product>());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext { ViewContext = new ViewContext { RouteData = new Microsoft.AspNetCore.Routing.RouteData() } };
            target.RouteData.Values["category"] = categoryToSelect;

            //action
            string? result = (string?)(target.Invoke() as ViewViewComponentResult)?.ViewData["SelectedCategory"];

            //assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
