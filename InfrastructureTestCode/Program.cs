var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// var version = Environment.GetEnvironmentVariable("APP_VERSION") ?? "1.0.0";
// app.MapGet("/", () => $"Hello World! version is {version}");
app.MapGet("/", () => $"Hello World! version is 1.0.0");

app.Run();