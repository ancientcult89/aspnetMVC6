namespace Platform
{
    public class WeatherMiddleware
    {
        private RequestDelegate _next;
        public WeatherMiddleware(RequestDelegate next)
        { 
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/middleware/class")
                await context.Response.WriteAsync("Mddleware Class: Its raining in London");
            else 
                await _next(context);
        }
    }
}
