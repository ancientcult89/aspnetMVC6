using Platform;
using Platform.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();

var app = builder.Build();

app.UseMiddleware<WeatherMiddleware>();

//IResponseFormatter formatter = new TextResponseFormatter();

//using by singleton
//app.MapGet("middleware/function", async context => {
//    await TextResponseFormatter.Singleton.Format(context, "Middleware Function: Its snowing in Chicago");
//});
//using by type broker
//app.MapGet("middleware/function", async context => {
//    await TypeBroker.Formatter.Format(context, "Middleware Function: Its snowing in Chicago");
//});

//using by DI
app.MapGet("middleware/function", async (HttpContext context, IResponseFormatter formatter) => {
    await formatter.Format(context, "Middleware Function: Its snowing in Chicago");
});

//app.MapGet("endpoint/class", WeatherEndpoint.Endpoint);
app.MapEndpoint<WeatherEndpoint>("endpoint/class");

app.MapGet("endpoint/function", async (HttpContext context, IResponseFormatter formatter) => {
    await formatter.Format(context, "Endpoint Function: Its sunny in LA");
});

app.Map("/", async context => { await context.Response.WriteAsync("Hello world yopta"); });

app.Run();