using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class SecondController : Controller
    {
        public IActionResult Index()
        { 
            return View("Common");
            return View("/Views/Shared/Common.cshtml");
        }
    }
}
