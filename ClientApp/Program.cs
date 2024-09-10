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
var codeRunServiceUrl = Environment.GetEnvironmentVariable("CODE_RUN_SERVICE_URL") ?? "http://localhost:5001";
var codeBaseServiceUrl = Environment.GetEnvironmentVariable("CODE_BASE_SERVICE_URL") ?? "http://localhost:5002";
var userServiceUrl = Environment.GetEnvironmentVariable("USER_SERVICE_URL") ?? "http://localhost:5003";
var formatterServiceUrl = Environment.GetEnvironmentVariable("FORMATTER_SERVICE_URL") ?? "http://localhost:5004";
var executionEngineUrl = Environment.GetEnvironmentVariable("EXECUTION_SERVICE_URL") ?? "http://localhost:5005";

// Create and register the ServiceUrls instance
var serviceUrls = new ServiceUrls
{
    CodeRunServiceUrl = codeRunServiceUrl,
    CodeBaseServiceUrl = codeBaseServiceUrl,
    UserServiceUrl = userServiceUrl,
    FormatterServiceUrl = formatterServiceUrl,
    ExecutionEngineUrl = executionEngineUrl
};

// Register ServiceUrls as a singleton or scoped service
builder.Services.AddSingleton(serviceUrls);

await builder.Build().RunAsync();