//using Microsoft.Extensions.Options;
using Platform;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageOption>(options => { options.CityName = "Albany"; });
builder.Services.Configure<RouteOptions>(opts => { opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint)); });

var app = builder.Build();

app.Use(async (context, next) => { 
    Endpoint? end = context.GetEndpoint();
    if (end != null)
    {
        await context.Response.WriteAsync($"{end.DisplayName} Selected\n");
    }
    else
        await context.Response.WriteAsync("No endpoint selected\n");
    await next();
});

app.Map("{number:int}", async context => { await context.Response.WriteAsync("Routed to the int endpoint"); })
    .WithDisplayName("Int endpoint").Add(b => ((RouteEndpointBuilder)b).Order = 1);
app.Map("{number:double}", async context => { await context.Response.WriteAsync("Routed to the double endpoint"); })
    .WithDisplayName("Double endpoint").Add(b => ((RouteEndpointBuilder)b).Order = 2);

app.MapGet("{first:alpha:length(3)}/{second:bool}", async context => {
    await context.Response.WriteAsync("Request was Routed\n");
    foreach (var kvp in context.Request.RouteValues)
    {
        await context.Response.WriteAsync($"{kvp.Key} : {kvp.Value}\n");
    }
});

app.MapGet("capital/{country:countryName}", Capital.Endpoint);
app.MapGet("size/{city?}", Population.Endpoint);
app.MapFallback(async context => { await context.Response.WriteAsync("Routed to fallback endpoint"); });

app.Run();