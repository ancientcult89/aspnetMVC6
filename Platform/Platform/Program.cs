using Platform;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(opts => {
    opts.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath | HttpLoggingFields.ResponseStatusCode;
});

var app = builder.Build();

app.UseHttpLogging();
//var logger = app.Services.GetService<ILoggerFactory>().CreateLogger("Pipeline");

//logger.LogDebug("Pipeline configuration starting");
app.MapGet("population/{city?}", Population.Endpoint);
//logger.LogDebug("Pipeline configuration complete");

app.MapGet("/", async context => {
    await context.Response.WriteAsync("Hello world!");
});

app.Run();