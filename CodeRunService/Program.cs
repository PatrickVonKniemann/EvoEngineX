using CodeRunService;
using CodeRunService.Application.Services;
using CodeRunService.Consumers;
using CodeRunService.Infrastructure;
using CodeRunService.Infrastructure.Database;
using Common;
using DomainEntities;
using FastEndpoints;
using FastEndpoints.Swagger;
using Helpers;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Code Run Service API";
            s.Version = "v0.0.1";
        };
    });

// Configure logging explicitly
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddLogging();
builder.Services.AddSignalR();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddScoped<ICodeRunCommandService, CodeRunCommandService>();
builder.Services.AddScoped<ICodeRunQueryService, CodeRunQueryService>();
builder.Services.AddScoped<ICodeRunRepository, CodeRunRepository>();
builder.Services.AddScoped<ICodeExecutionCommandService, CodeExecutionCommandService>();
builder.Services.AddScoped<ICodeValidationService, CodeValidationService>();

builder.Services.AddAutoMapper(cg => cg.AddProfile(new CodeRunProfile()));

// Get RabbitMQ settings from environment variables
var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "kolenpat";
var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "sa";
var rabbitMqPort = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
    new ConnectionFactory
    {
        HostName = rabbitMqHost,
        UserName = rabbitMqUser,
        Password = rabbitMqPass,
        Port = int.Parse(rabbitMqPort)
    });

builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddHostedService<CodeValidationConsumer>();
builder.Services.AddHostedService<CodeExecutionConsumer>();

// Setup database connection
var connectionString = builder.Configuration.GetConnectionString("CodeRunDatabase");
connectionString = connectionString?
    .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost:5433")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "CodeRunDb")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "kolenpat")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa");

builder.Services.AddDbContext<CodeRunDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();
app.Logger.LogInformation("Using connection string: {ConnectionString}", connectionString);
app.Logger.LogInformation(
    $"Using connection RabbitMQ connecting: {rabbitMqHost}, {rabbitMqUser}, {rabbitMqPass}, {rabbitMqPort}");

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CodeRunDbContext>();
        await context.Database.EnsureCreatedAsync();
        app.Logger.LogInformation("Database migrations applied successfully");

        // Check if seeding is needed based on environment
        var fileList = new List<string>
        {
            "CodeRuns"
        };
        var sqlDirectory = "../Configs/SqlScripts";
        await DbHelper.RunSeedSqlFileAsync(sqlDirectory, app.Logger, connectionString, fileList);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while applying migrations");
    }
}

app.UseMiddleware<ErrorHandlingMiddleware>(); // Use custom middleware
app.UseFastEndpoints()
    .UseSwaggerGen();

app.MapHub<CodeRunHub>("/codeRunHub");

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

await app.RunAsync();

/// <summary>
/// This class is used to start the API,
/// Partial class is used to add the entry point for CustomWebApplicationFactory
/// </summary>
public abstract partial class Program;