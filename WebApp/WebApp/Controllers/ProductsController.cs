using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private DataContext _dataContext;
        public ProductsController(DataContext dataContext) => _dataContext = dataContext;

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return _dataContext.Products.AsAsyncEnumerable(); ;
        }

        [HttpGet("{id}")]
        public async Task<Product?> GetProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            return await _dataContext.Products.FindAsync(id);
        }

        [HttpPost]
        public async Task SaveProduct([FromBody] Product product)
        { 
            await _dataContext.Products.AddAsync(product);
            await _dataContext.SaveChangesAsync();
        }

        [HttpPut]
        public async Task UpdateProduct([FromBody] Product product)
        {
            _dataContext.Products.Update(product);
            await _dataContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id, [FromServices] ILogger<ProductsController> logger)
        {
            _dataContext.Products.Remove(new Product() { ProductId = id });
            await _dataContext.SaveChangesAsync();
        }
    }
}
