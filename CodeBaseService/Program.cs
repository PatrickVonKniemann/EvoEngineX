using CodebaseService.Application.Services;
using CodeBaseService.Application.Services;
using CodeBaseService.Infrastructure;
using CodebaseService.Infrastructure.Database;
using Common;
using DomainEntities;
using FastEndpoints;
using FastEndpoints.Swagger;
using Helpers;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Environment-specific configuration
var environment = builder.Environment.EnvironmentName;

// Configure logging explicitly based on environment
ConfigureLogging(builder.Logging, environment);

// Add services to the container
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Log connection string
app.Logger.LogInformation("Using connection string: {ConnectionString}", GetPostgresConnectionString(builder.Configuration));

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
                s.Title = "Code Base Service API";
                s.Version = "v0.0.1";
            };
        });

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            corsPolicyBuilder => corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    // Register application services
    services.AddScoped<ICodeBaseCommandService, CodeBaseCommandService>();
    services.AddScoped<ICodeBaseQueryService, CodeBaseQueryService>();
    services.AddScoped<ICodeBaseRepository, CodeBaseRepository>();

    services.AddAutoMapper(cfg => cfg.AddProfile(new CodebaseProfile()));

    // Setup PostgreSQL database connection
    var connectionString = GetPostgresConnectionString(configuration);
    services.AddDbContext<CodeBaseDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Setup MongoDB
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

string GetPostgresConnectionString(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("CodeBaseDatabase");

    return connectionString?.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
        .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "CodeBaseServiceDb")
        .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "5433")
        .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "kolenpat")
        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa") ?? throw new InvalidOperationException("No connection string provide for DB");
}

MongoClient CreateMongoClient(string mongoDatabaseName)
{
    var mongoConnectionString = $"mongodb://{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_USERNAME") ?? "kolenpat"}:{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_PASSWORD") ?? "sa"}@{Environment.GetEnvironmentVariable("MONGO_HOST") ?? "mongo"}:{Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27017"}/{mongoDatabaseName}";

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
        throw; // Optionally, you can decide if you want to stop the app if MongoDB is not reachable
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
        var context = services.GetRequiredService<CodeBaseDbContext>();
        await context.Database.EnsureCreatedAsync();
        appRuntime.Logger.LogInformation("Database migrations applied successfully");

        // Seed the database if necessary
        var sqlDirectory = "./SqlScripts";
        var fileList = new List<string> { "CodeBases" };
        await DbHelper.RunSeedSqlFileAsync(sqlDirectory, appRuntime.Logger,
            GetPostgresConnectionString(appRuntime.Configuration), fileList);
    }
    catch (Exception ex)
    {
        appRuntime.Logger.LogError(ex, "An error occurred while applying migrations");
    }
}

void ConfigureMiddleware(WebApplication appRuntime)
{
    appRuntime.UseMiddleware<ErrorHandlingMiddleware>(); // Custom error handling middleware
    appRuntime.UseFastEndpoints();
    appRuntime.UseSwaggerGen();
    appRuntime.UseHttpsRedirection();
    appRuntime.UseCors("AllowAllOrigins");
}

/// <summary>
/// Partial class used to allow for test entry points or other extensions.
/// </summary>
public abstract partial class Program;
