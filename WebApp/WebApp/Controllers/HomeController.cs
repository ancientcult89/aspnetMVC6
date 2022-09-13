using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _dataContext;
        public HomeController(DataContext dataContext)
        { 
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index(long id = 1)
        {
            Product? prod = await _dataContext.Products.FindAsync(id);
            if (prod?.CategoryId == 1)
                return View("Watersports", prod);
            else
                return View(prod);
        }

        public IActionResult Common()
        { 
            return View();
        }
    }
}
