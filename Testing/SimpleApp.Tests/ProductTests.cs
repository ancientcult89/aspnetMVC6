using SimpleApp.Models;
using Xunit;


namespace SimpleApp.Tests
{
    public class ProductTests
    {
        // a/a/a testing pattern
        [Fact]
        public void CanChangeProductName()
        {
            //arrange
            var p = new Product { Name = "Test", Price = 100M };
            string newName;

            //act
            p.Name = newName = "New name";

            //assert
            Assert.Equal(newName, p.Name);
        }

        [Fact]
        public void CanChangeProductPrice()
        {
            //arrange
            var p = new Product { Name = "Test", Price = 100M };
            decimal newPrice;

            //act
            p.Price = newPrice = 200M;

            //assert
            Assert.Equal(newPrice, p.Price);
        }
    }
}
