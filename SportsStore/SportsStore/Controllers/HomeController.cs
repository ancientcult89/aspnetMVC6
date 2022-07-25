using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;
        public int pageSize = 3;

        public HomeController(IStoreRepository repository)
        { 
            _repository = repository;
        }
        public IActionResult Index(int productPage = 1) => View(new ProductListViewModel
        {
            Products = _repository.Products.OrderBy(p => p.ProductId).Skip((productPage - 1) * pageSize).Take(pageSize),
            PageInfo = new PageInfo {
                CurrentPage = productPage,
                ItensPerPage = pageSize,
                TotalItems = _repository.Products.Count()
            }
        });
    }
}
