using CodeFormaterService.Consumers;
using CodeFormaterService.Services;
using Common;
using FastEndpoints;
using FastEndpoints.Swagger;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Environment-specific configuration
var environment = builder.Environment.EnvironmentName;

// Configure logging explicitly based on environment
ConfigureLogging(builder.Logging, environment);

// Add services to the container
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app);

// --------------------------
// Application starting point
// --------------------------
await app.RunAsync();

// --------------------------
// Application methods
// --------------------------
void ConfigureLogging(ILoggingBuilder loggingBuilder, string profileEnvironment)
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();

    if (profileEnvironment == "Development")
    {
        loggingBuilder.AddDebug(); // Add debug-level logging for development
    }

    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Error);
}

void ConfigureServices(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddFastEndpoints();
    services.AddSwaggerGen();

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            corsPolicyBuilder => corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    services.AddScoped<ICodeValidationService, CodeValidationService>();

    // Get RabbitMQ settings from environment variables
    var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "kolenpat";
    var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "sa";
    var rabbitMqPort = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";

    services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
        new ConnectionFactory
        {
            HostName = rabbitMqHost,
            UserName = rabbitMqUser,
            Password = rabbitMqPass,
            Port = int.Parse(rabbitMqPort)
        });

    services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
    services.AddSingleton<ICodeValidationService, CodeValidationService>();
    services.AddHostedService<CodeValidationRequestConsumer>();
}

void ConfigureMiddleware(WebApplication appRuntime)
{
    if (appRuntime.Environment.IsDevelopment())
    {
        appRuntime.UseDeveloperExceptionPage();
        appRuntime.UseSwagger();
        appRuntime.UseSwaggerUI();
    }

    appRuntime.UseMiddleware<ErrorHandlingMiddleware>(); // Use custom error handling middleware
    appRuntime.UseFastEndpoints()
        .UseSwaggerGen();

    appRuntime.UseHttpsRedirection();
    appRuntime.UseCors("AllowAllOrigins");
}

/// <summary>
/// Partial class used to allow for test entry points or other extensions.
/// </summary>
public abstract partial class Program;
