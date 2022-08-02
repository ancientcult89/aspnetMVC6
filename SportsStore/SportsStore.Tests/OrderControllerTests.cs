using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        { 
            //arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            //act
            ViewResult? result = target.Checkout(order) as ViewResult;

            //assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("error", "error");

            //act
            ViewResult? result = target.Checkout(new Order()) as ViewResult;

            //assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController target = new OrderController(mock.Object, cart);

            //act
            RedirectToPageResult? result = target.Checkout(new Order()) as RedirectToPageResult;

            //assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("/Completed", result?.PageName);
        }
    }
}
