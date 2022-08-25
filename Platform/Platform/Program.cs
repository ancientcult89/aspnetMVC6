using Platform;
//using Platform.Services;

var builder = WebApplication.CreateBuilder(args);

var servicesConfig = builder.Configuration;
builder.Services.Configure<MessageOption>(servicesConfig.GetSection("Location"));

var serviceEnv = builder.Environment;

var app = builder.Build();

var pipelineConfig = app.Configuration;

var piplineEnv = app.Environment;

app.UseMiddleware<LocationMiddleware>();

app.MapGet("config", async (HttpContext context, IConfiguration config, IWebHostEnvironment env) => {
    string defaultDebug = config["Logging:Loglevel:Default"];
    await context.Response.WriteAsync($"The config setting is: {defaultDebug}");
    await context.Response.WriteAsync($"\nThe env setting is: {env.EnvironmentName}");
    string wsID = config["WebService:Id"];
    string wsKey = config["WebService:Key"];
    await context.Response.WriteAsync($"\nThe secret ID is: {wsID}");
    await context.Response.WriteAsync($"\nThe secret Key is: {wsKey}");
});

app.MapGet("/", async context => {
    await context.Response.WriteAsync("Hello world!");
});

app.Run();