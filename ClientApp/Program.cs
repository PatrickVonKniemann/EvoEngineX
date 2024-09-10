using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientApp;
using ClientApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<CodeFormatService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CodeBaseService>();
builder.Services.AddScoped<CodeRunService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<CodeRunStatusConnectorService>();

// Read environment variables passed from Docker Compose
var codeRunServiceUrl = Environment.GetEnvironmentVariable("CODE_RUN_SERVICE_SERVICE_URL");
var codeBaseServiceUrl = Environment.GetEnvironmentVariable("CODE_BASE_SERVICE_SERVICE_URL");
var userServiceUrl = Environment.GetEnvironmentVariable("USER_SERVICE_URL");
var formatterServiceUrl = Environment.GetEnvironmentVariable("FORMATTER_SERVICE_URL");
var executionEngineUrl = Environment.GetEnvironmentVariable("EXECUTION_SERVICE_URL");


await builder.Build().RunAsync();