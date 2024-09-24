var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var version = Environment.GetEnvironmentVariable("APP_VERSION") ?? "1.0.0";
app.MapGet("/appversion", () => $"Hello World! version is {version}");
app.Run();