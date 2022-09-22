using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Html;

namespace WebApp.Components
{
    public class CitySummary : ViewComponent
    {
        private CitiesData _cityData;
        public CitySummary(CitiesData data)
        { 
            _cityData = data;
        }

        public IViewComponentResult Invoke(string themeName = "success")
        {
            ViewBag.Theme = themeName;
            return View(new CityViewModel
            {
                Cities = _cityData.Cities.Count(),
                Population = _cityData.Cities.Sum(c => c.Population)
            });
            //return new HtmlContentViewComponentResult(new HtmlString("This is a <h3><i>string</i></h3>"));
        }

        //public string Invoke()
        //{
        //    if (RouteData.Values["controller"] != null)
        //        return "Controller request";
        //    else
        //        return "RazorPage request";
        //}
    }
}
