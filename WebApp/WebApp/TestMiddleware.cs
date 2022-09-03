using WebApp.Models;

namespace WebApp
{
    public class TestMiddleware
    {
        private RequestDelegate _nextDelegate;
        public TestMiddleware(RequestDelegate next)
        { 
            _nextDelegate = next;
        }

        public async Task Invoke(HttpContext context, DataContext dataContext)
        {
            if (context.Request.Path == "/test")
            {
                await context.Response.WriteAsync($"There are {dataContext.Products.Count()} products\n");
                await context.Response.WriteAsync($"There are {dataContext.Categories.Count()} categories\n");
                await context.Response.WriteAsync($"There are {dataContext.Suppliers.Count()} suppliers\n");
            }
            else
                _nextDelegate(context);
        }
    }
}
