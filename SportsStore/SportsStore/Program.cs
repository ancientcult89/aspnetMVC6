using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(options => 
    options.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]));

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

builder.Services.AddRazorPages();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//for supporting wwwroot folder
app.UseStaticFiles();
app.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", action = "Index" });
app.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });
app.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 });
app.MapControllerRoute("pagination",
    "Products/Page{productPage}",
    new { Controller = "Home", action = "Index", productPage = 1});
app.MapDefaultControllerRoute();
app.MapRazorPages();

SeedData.EnsurePopulated(app);

app.Run();
