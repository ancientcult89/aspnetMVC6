using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Advanced.Models;
using Advanced.Services;

namespace Advanced.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _dataContext;
        private ToggleService _toggleService;
        public HomeController(DataContext dataContext, ToggleService ts) { 
            _dataContext = dataContext;
            _toggleService = ts;
        }
        public IActionResult Index([FromQuery] string selectedCity)
        {
            return View(new PeopleListViewModel { 
                People = _dataContext.People.Include(p => p.Location),
                Cities = _dataContext.Locations.Select(l => l.City).Distinct(),
                SelectedCity = selectedCity
            });
        }

        public string Toggle() => $"Enabled: {_toggleService.ToggleComponents()}";
    }

    public class PeopleListViewModel
    {
        public IEnumerable<Person> People { get; set; } = Enumerable.Empty<Person>();
        public IEnumerable<string> Cities { get; set; } = Enumerable.Empty<string>();
        public string SelectedCity { get; set; } = String.Empty;
        public string GetClass(string? city) =>
            SelectedCity == city ? "bg-info text-white" : "";
    }
}
