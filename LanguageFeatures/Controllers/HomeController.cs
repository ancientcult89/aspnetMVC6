//using Microsoft.AspNetCore.Mvc;
//using LanguageFeatures.Models;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //Product?[] products = Product.GetProducts();
            //Product? p = products[0];
            //string val;
            //val = p != null ? p.Name : "No value";

            //string? val = products[0]?.Name;

            //if(val != null)
            //    return View(new string[] { val });
            //return View(new string[] { "No value" });

            //return View(new string[] { products[0]?.Name ?? "No values" });

            //Product?[] products = Product.GetProducts();
            //return View(new string[] { 
            //    $"Name: { products[0]?.Name }, Price: { products[0]?.Price }"
            //});

            //string[] names = new string[3];
            //names[0] = "Bob";
            //names[1] = "Joe";
            //names[2] = "Alice";
            //return View("Index", names);

            //return View("Index", new string[] { "Bob", "Joe", "Alice" });

            //Dictionary<string, Product> products = new Dictionary<string, Product> {
            //    { "Kayak", new Product { Name = "Kayak", Price = 275M } },
            //    { "Lifejacket", new Product{ Name = "Lifejacket", Price = 48.95M } }
            //};
            //return View("Index", products.Keys);

            //Dictionary<string, Product> products = new Dictionary<string, Product>
            //{
            //    ["Kayak"] = new Product { Name = "Kayak", Price = 275M },
            //    ["Lifejacket"] = new Product { Name = "Lifejacket", Price = 48.95M }
            //};
            //return View("Index", products.Keys);

            object[] data = new object[] { 275M, 29.95M,
                "apple", "orange", 100, 10 };
            decimal total = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] is decimal d)
                {
                    total += d;
                }
            }
            return View("Index", new string[] { $"Total: {total:C2}" });
        }
    }
}
