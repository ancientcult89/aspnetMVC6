//using Platform;
//using Platform.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(ICollection<>), typeof(List<>));
//builder.Services.AddScoped<IResponseFormatter, TimeResponseFormatter>();
//builder.Services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();
//builder.Services.AddScoped<IResponseFormatter, GuidService>();
//builder.Services.AddScoped<ITimeStamper, DefaultTimeStamper>();


//IWebHostEnvironment env = builder.Environment;
//if(env.IsDevelopment())
//{
//    builder.Services.AddScoped<IResponseFormatter, TimeResponseFormatter>();
//    builder.Services.AddScoped<ITimeStamper, DefaultTimeStamper>();
//}
//else
//    builder.Services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();

//IConfiguration config = builder.Configuration;
//builder.Services.AddScoped<IResponseFormatter>(serviceProvider => {
//    string? typeName = config["services:IResponseFormatter"];
//    return (IResponseFormatter)ActivatorUtilities.CreateInstance(serviceProvider, 
//        typeName == null ? typeof(GuidService) : Type.GetType(typeName, true)!);
//});
//builder.Services.AddScoped<ITimeStamper, DefaultTimeStamper>();

//builder.Services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();

var app = builder.Build();

//app.UseMiddleware<WeatherMiddleware>();

//IResponseFormatter formatter = new TextResponseFormatter();

//using by singleton
//app.MapGet("middleware/function", async context => {
//    await TextResponseFormatter.Singleton.Format(context, "Middleware Function: Its snowing in Chicago");
//});
//using by type broker
//app.MapGet("middleware/function", async context => {
//    await TypeBroker.Formatter.Format(context, "Middleware Function: Its snowing in Chicago");
//});

////using by DI
//app.MapGet("middleware/function", async (HttpContext context, IResponseFormatter formatter) => {
//    await formatter.Format(context, "Middleware Function: Its snowing in Chicago");
//});

////app.MapGet("endpoint/class", WeatherEndpoint.Endpoint);
//app.MapEndpoint<WeatherEndpoint>("endpoint/class");

//app.MapGet("endpoint/function", async (HttpContext context) => {
//    IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
//    await formatter.Format(context, "Endpoint Function: Its sunny in LA");
//});

//app.MapGet("single", async context => { 
//    IResponseFormatter formatter = context.RequestServices.GetRequiredService<IResponseFormatter>();
//    await formatter.Format(context, "Single service");
//});

//app.Map("/", async context => {
//    IResponseFormatter formatter = context.RequestServices.GetServices<IResponseFormatter>().First(f => f.RichOutput);
//    await formatter.Format(context, "Multiple service");
//});

app.MapGet("string", async context => { 
    ICollection<string> collection = context.RequestServices.GetRequiredService<ICollection<string>>();
    collection.Add($"Request: {DateTime.Now.ToLongTimeString()}");
    foreach (string str in collection)
    { 
        await context.Response.WriteAsync($"String: {str}\n");
    }
});

app.MapGet("int", async context => {
    ICollection<int> collection = context.RequestServices.GetRequiredService<ICollection<int>>();
    collection.Add(collection.Count + 1);
    foreach (int val in collection)
    {
        await context.Response.WriteAsync($"Int: {val}\n");
    }
});

app.Run();