namespace Platform
{
    public class WeatherEndpoint
    {
        public static async Task Endpoint(HttpContext context) =>
            await context.Response.WriteAsync("Endpoint Class: Its cloudy in Milan");
    }
}
