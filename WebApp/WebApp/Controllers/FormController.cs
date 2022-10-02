using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class FormController : Controller
    {
        private DataContext _dataContext;
        public FormController(DataContext dataContext)
        { 
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(long id = 1)
        {
            List<Category> categories = _dataContext.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View("Form", await _dataContext.Products
                .Include(p => p.Category).Include(p => p.Supplier)
                .FirstAsync(p => p.ProductId == id));
        }

        public IActionResult SubmitForm()
        {
            foreach (string key in Request.Form.Keys)
            {
                TempData[key] = string.Join(", ", Request.Form[key]);
            }
            return RedirectToAction(nameof(Results));
        }

        public IActionResult Results()
        { 
            return View();
        }
    }
}
