using WebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private DataContext _dataContext;
        public Product? Product { get; set; }
        public IndexModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
  
        public async Task<IActionResult> OnGetAsync(long id = 1)
        {
            Product = await _dataContext.Products.FindAsync(id);
            if(Product == null)
                return RedirectToPage("NotFound");
            return Page();
        }
    }
}