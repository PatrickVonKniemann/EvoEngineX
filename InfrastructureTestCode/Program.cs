var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var version = "1.0.0.";
app.MapGet("/", () => $"Hello World! version is {version}");

app.Run();