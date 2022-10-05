using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationController : ControllerBase
    {
        private DataContext _dataContext;

        public ValidationController(DataContext dataContext)
        { 
            _dataContext = dataContext;
        }
        [HttpGet("categorykey")]
        public bool CategoryKey(string? categoryId, [FromQuery] KeyTarget target)
        {
            long keyVal;
            return long.TryParse(categoryId ?? target.CategoryId, out keyVal) && _dataContext.Categories.Find(keyVal) != null;
        }

        [HttpGet("supplierkey")]
        public bool SuplierKey(string? supplierId, [FromQuery] KeyTarget target)
        {
            long keyVal;
            return long.TryParse(supplierId ?? target.SupplierId, out keyVal) && _dataContext.Suppliers.Find(keyVal) != null;
        }

        [Bind(Prefix = "Product")]
        public class KeyTarget
        { 
            public string CategoryId { get; set; }
            public string SupplierId { get; set; }
        }
    }
}
