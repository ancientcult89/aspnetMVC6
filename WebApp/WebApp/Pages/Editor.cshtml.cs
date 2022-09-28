using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages
{
    public class EditorModel : PageModel
    {
        private DataContext _dataContext;
        public Product? Product { get; set; } = new();
        public EditorModel(DataContext ctx)
        { 
            _dataContext = ctx;
        }

        public async Task OnGetAsync(long id = 1)
        { 
            Product = await _dataContext.Products.FindAsync(id) ?? new();
        }

        public async Task<IActionResult> OnPostAsync(long id, decimal price)
        { 
            Product? p = await _dataContext.Products.FindAsync(id);
            if (p != null)
                p.Price = price;
            await _dataContext.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
