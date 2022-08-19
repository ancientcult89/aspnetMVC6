using Microsoft.Extensions.Options;

namespace Platform
{
    public class QueryStringMiddleware
    {
        private RequestDelegate? _next;
        public QueryStringMiddleware(){ }

        public QueryStringMiddleware(RequestDelegate nextDelegate)
        {
            _next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get && context.Request.Query["custom"] == "true")
            {
                if(!context.Response.HasStarted)
                    context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Class-based Middlware \n");
            }
            if(_next != null)
                await _next(context);
        }
    }

    public class LocationMiddleware 
    {
        private RequestDelegate _next;
        private MessageOption _option;

        public LocationMiddleware(RequestDelegate nextDelegate, IOptions<MessageOption> opts)
        {
            _next = nextDelegate;
            _option = opts.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/location")
                await context.Response.WriteAsync($"{_option.CityName}, {_option.CountryName}");
            else 
                await _next(context);
        }
    }
}
