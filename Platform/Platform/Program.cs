using Platform;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opts => { 
    opts.IdleTimeout = TimeSpan.FromMinutes(30);
    opts.Cookie.IsEssential = true;
});

builder.Services.AddHsts(opts => { 
    opts.MaxAge = TimeSpan.FromDays(1);
    opts.IncludeSubDomains = true;
});

builder.Services.Configure<CookiePolicyOptions>(opts => {
    opts.CheckConsentNeeded = context => true;
});
builder.Services.AddHttpsRedirection(opts => {
    opts.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    opts.HttpsPort = 443;
});

var app = builder.Build();
if(app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();
app.UseSession();

app.UseCookiePolicy();
app.UseMiddleware<ConsentMiddleware>();

app.MapGet("/session", async context => {
    int counter1 = (context.Session.GetInt32("counter1") ?? 0) + 1;
    int counter2 = (context.Session.GetInt32("counter2") ?? 0) + 1;
    context.Session.SetInt32("counter1", counter1);
    context.Session.SetInt32("counter2", counter2);
    await context.Session.CommitAsync();
    await context.Response.WriteAsync($"Counter1: {counter1}, Counter2: {counter2}");
});

app.MapGet("clear", context => {
    context.Response.Cookies.Delete("counter1");
    context.Response.Cookies.Delete("counter2");
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.MapFallback(async context => {
    await context.Response.WriteAsync($"HTTPS Request: {context.Request.IsHttps}\n");
    await context.Response.WriteAsync("Hello world!");
});

app.Run();