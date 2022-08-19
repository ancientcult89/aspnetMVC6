//using Microsoft.Extensions.Options;
using Platform;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageOption>(options => { options.CityName = "Albany"; });

var app = builder.Build();

//app.MapGet("/location", async (HttpContext context, IOptions<MessageOption> msgOpts) => {
//    Platform.MessageOption opts = msgOpts.Value;
//    await context.Response.WriteAsync($"{opts.CityName}, {opts.CountryName}");
//});
app.UseMiddleware<LocationMiddleware>();

((IApplicationBuilder)app).Map("/branch", branch => {
    branch.Run(new Platform.QueryStringMiddleware().Invoke);
});

app.Use(async (context, next) => {
    await next();
    await context.Response.WriteAsync($"\nStatus code: {context.Response.StatusCode}");
});

app.Use(async (context, next) => {
    if (context.Request.Path == "/short")
        await context.Response.WriteAsync($"Request Short Circuited");
    else
        await next();
});

app.Use(async (context, next) =>{
    if (context.Request.Method == HttpMethods.Get && context.Request.Query["custom"] == "true")
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Custom Middlware \n");
    }
    await next();
});

app.UseMiddleware<Platform.QueryStringMiddleware>();

app.MapGet("/", () => "Hello World!");

app.Run();
