//using Microsoft.Extensions.Options;
using Platform;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageOption>(options => { options.CityName = "Albany"; });

var app = builder.Build();

app.MapGet("files/{filename}.{ext}", async context => {
    await context.Response.WriteAsync("Request was Routed\n");
    foreach (var kvp in context.Request.RouteValues)
    {
        await context.Response.WriteAsync($"{kvp.Key} : {kvp.Value}\n");
    }
});

app.MapGet("capital/{country}", Capital.Endpoint);
app.MapGet("size/{city}", Population.Endpoint).WithMetadata(new RouteNameMetadata("population"));

app.Run();