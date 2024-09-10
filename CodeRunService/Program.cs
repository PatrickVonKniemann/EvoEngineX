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
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Environment-specific configuration
var environment = builder.Environment.EnvironmentName;

// Configure logging explicitly based on environment
ConfigureLogging(builder.Logging, environment);

// Add services to the container
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Log connection strings
LogConnectionDetails(app);

// Apply database migrations and seeding
await ApplyDatabaseMigrationsAndSeedingAsync(app);

ConfigureMiddleware(app);

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

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddFastEndpoints()
        .SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "Code Run Service API";
                s.Version = "v0.0.1";
            };
        });

    services.AddSignalR();  // SignalR services
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

    // Register application services
    services.AddScoped<ICodeRunCommandService, CodeRunCommandService>();
    services.AddScoped<ICodeRunQueryService, CodeRunQueryService>();
    services.AddScoped<ICodeRunRepository, CodeRunRepository>();
    services.AddScoped<ICodeExecutionCommandService, CodeExecutionCommandService>();
    services.AddScoped<ICodeValidationService, CodeValidationService>();
    services.AddAutoMapper(cfg => cfg.AddProfile(new CodeRunProfile()));

    // RabbitMQ configuration
    var rabbitMqSettings = GetRabbitMqSettings();
    services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
        new ConnectionFactory
        {
            HostName = rabbitMqSettings.Host,
            UserName = rabbitMqSettings.User,
            Password = rabbitMqSettings.Pass,
            Port = rabbitMqSettings.Port
        });

    services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
    services.AddHostedService<CodeValidationConsumer>();
    services.AddHostedService<CodeExecutionConsumer>();

    // PostgreSQL configuration
    var postgresConnectionString = GetPostgresConnectionString(configuration);
    services.AddDbContext<CodeRunDbContext>(options =>
        options.UseNpgsql(postgresConnectionString));

    // MongoDB configuration
    var mongoDatabaseName = GetMongoDatabaseName();
    services.AddSingleton<IMongoClient, MongoClient>(sp =>
    {
        return CreateMongoClient(mongoDatabaseName);
    });

    services.AddSingleton(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        return client.GetDatabase(mongoDatabaseName);
    });
}

(string Host, string User, string Pass, int Port) GetRabbitMqSettings()
{
    return (
        Host: Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
        User: Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "kolenpat",
        Pass: Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "sa",
        Port: int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672")
    );
}

string GetPostgresConnectionString(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("CodeRunDatabase");

    return connectionString?.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
        .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "5433")
        .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "CodeRunDb")
        .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "kolenpat")
        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa") ?? throw new InvalidOperationException("No connection string provided for DB connector");
}

MongoClient CreateMongoClient(string mongoDatabaseName)
{
    var mongoConnectionString = $"mongodb://{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_USERNAME") ?? "kolenpat"}:{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_PASSWORD") ?? "sa"}@{Environment.GetEnvironmentVariable("MONGO_HOST") ?? "localhost"}:{Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27018"}/{mongoDatabaseName}";

    var mongoClient = new MongoClient(mongoConnectionString);

    // Try to ping the MongoDB server to check the connection
    try
    {
        var database = mongoClient.GetDatabase(mongoDatabaseName);
        var pingResult = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Result;

        if (pingResult != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"info: Successfully connected to MongoDB at {mongoConnectionString}");
            Console.ResetColor(); // Reset to default color
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"error: Failed to connect to MongoDB at {mongoConnectionString}: " + ex.Message);
        Console.ResetColor(); // Reset to default color
        throw; // Optionally, stop the app if MongoDB is not reachable
    }

    return mongoClient;
}

string GetMongoDatabaseName()
{
    return Environment.GetEnvironmentVariable("MONGO_DB") ?? "evoenginex_db";
}

async Task ApplyDatabaseMigrationsAndSeedingAsync(WebApplication appRuntime)
{
    using var scope = appRuntime.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<CodeRunDbContext>();
        await context.Database.EnsureCreatedAsync();
        appRuntime.Logger.LogInformation("Database created successfully");

        // Seed the database if necessary
        var sqlDirectory = "./SqlScripts";
        var fileList = new List<string> { "CodeRuns" };
        await DbHelper.RunSeedSqlFileAsync(sqlDirectory, appRuntime.Logger,
            GetPostgresConnectionString(appRuntime.Configuration), fileList);
    }
    catch (Exception ex)
    {
        appRuntime.Logger.LogError(ex, "An error occurred while applying migrations or seeding the database");
    }
}

void LogConnectionDetails(WebApplication appRuntime)
{
    var rabbitMqSettings = GetRabbitMqSettings();
    appRuntime.Logger.LogInformation("Using connection string: {ConnectionString}", GetPostgresConnectionString(appRuntime.Configuration));
    appRuntime.Logger.LogInformation("RabbitMQ connection: Host={Host}, User={User}, Port={Port}",
        rabbitMqSettings.Host, rabbitMqSettings.User, rabbitMqSettings.Port);
}

void ConfigureMiddleware(WebApplication appRuntime)
{
    appRuntime.UseMiddleware<ErrorHandlingMiddleware>(); // Custom error handling middleware
    appRuntime.UseFastEndpoints();
    appRuntime.UseSwaggerGen();
    appRuntime.MapHub<CodeRunHub>("/codeRunHub");
    appRuntime.UseHttpsRedirection();
    appRuntime.UseCors("AllowAllOrigins");
}

/// <summary>
/// Partial class used to allow for test entry points or other extensions.
/// </summary>
public abstract partial class Program;
