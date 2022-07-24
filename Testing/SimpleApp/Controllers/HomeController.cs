using Microsoft.AspNetCore.Mvc;
using static SimpleApp.Models.Product;

namespace SimpleApp.Controllers
{
    public class HomeController : Controller
    {
        public IDataSource dataSource = new ProductDataSource();
        public IActionResult Index()
        {
            return View(dataSource.Products);
        }
    }
}
