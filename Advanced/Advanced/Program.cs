using Microsoft.EntityFrameworkCore;
using Advanced.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Advanced;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<Advanced.Services.ToggleService>();

builder.Services.AddDbContext<IdentityContext>(opts => 
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddDbContext<DataContext>(opts => {
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:PeopleConnection"]);
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.Configure<IdentityOptions>(opts => {
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
    opts.User.RequireUniqueEmail = true;
    opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
});

builder.Services.AddAuthentication(opts => {
    opts.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(opts => {
    opts.Events.DisableRedirectForPath(e => e.OnRedirectToLogin,
    "/api", StatusCodes.Status401Unauthorized);
    opts.Events.DisableRedirectForPath(e => e.OnRedirectToAccessDenied,
    "/api", StatusCodes.Status403Forbidden);
});

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute("controllers", "controllers/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseBlazorFrameworkFiles("/webassembly");
app.MapFallbackToFile("/webassembly/{*path:nonfile}", "/webassembly/index.html");

var context = app.Services.CreateScope().ServiceProvider
    .GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);
IdentitySeedData.CreateAdminAccount(app.Services, app.Configuration);

app.Run();
