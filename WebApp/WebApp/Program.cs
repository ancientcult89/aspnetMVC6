using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<DataContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

var app = builder.Build();

const string _baseurl = "/api/products";

app.MapGet($"{_baseurl}/{{id}}", async (HttpContext context, DataContext data) => {
    
    string? id = context.Request.RouteValues["id"] as string;
    if (id != null)
    {
        Product? p = data.Products.Find(long.Parse(id));
        if (p == null)
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        else
        { 
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize<Product>(p));
        }
    }
});

app.MapGet(_baseurl, async (HttpContext context, DataContext data) => { 
    context.Response.ContentType="application/json";
    await context.Response.WriteAsync(JsonSerializer.Serialize<IEnumerable<Product>>(data.Products));
});

app.MapPost(_baseurl, async (HttpContext context, DataContext data) => {
    Product? p = await JsonSerializer.DeserializeAsync<Product>(context.Request.Body);
    if (p != null)
    {
        await data.AddAsync(p);
        await data.SaveChangesAsync();
        context.Response.StatusCode = StatusCodes.Status200OK;
    }
});

app.UseMiddleware<WebApp.TestMiddleware>();

app.MapGet("/", () => "Hello World!");

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

SeedData.SeedDatabase(context);

app.Run();
