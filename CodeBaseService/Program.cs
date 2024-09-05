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

// Add services to the container.
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Code Base Service API";
            s.Version = "v0.0.1";
        };
    });

// Configure logging explicitly
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddLogging(); // Ensure logging is added first

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddScoped<ICodeBaseCommandService, CodeBaseCommandService>();
builder.Services.AddScoped<ICodeBaseQueryService, CodeBaseQueryService>();
builder.Services.AddScoped<ICodeBaseRepository, CodeBaseRepository>();

builder.Services.AddAutoMapper(cg => cg.AddProfile(new CodebaseProfile()));

var connectionString = builder.Configuration.GetConnectionString("CodeBaseDatabase");
connectionString = connectionString?.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "CodeBaseServiceDb")
    .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "5433")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "kolenpat")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa");

builder.Services.AddDbContext<CodeBaseDbContext>(options =>
    options.UseNpgsql(connectionString));

// Get MongoDB connection settings from environment variables
var mongoConnectionString = $"mongodb://{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_USERNAME") ?? "kolenpat"}:{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_PASSWORD") ?? "sa"}@{Environment.GetEnvironmentVariable("MONGO_HOST") ?? "mongo"}:{Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27017"}/{Environment.GetEnvironmentVariable("MONGO_DB") ?? "evoenginex_db"}";
var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGO_DB") ?? "evoenginex_db";

// Register MongoDB client as a singleton
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var mongoClient = new MongoClient(mongoConnectionString);

    // Try to ping the MongoDB server to check the connection
    try
    {
        var database = mongoClient.GetDatabase(mongoDatabaseName);
        var pingResult = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Result;

        if (pingResult != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Successfully connected to MongoDB at {mongoConnectionString}");
            Console.ResetColor(); // Reset to default color
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to connect to MongoDB at {mongoConnectionString}: " + ex.Message);
        Console.ResetColor(); // Reset to default color
        throw; // Optionally, you can decide if you want to stop the app if MongoDB is not reachable
    }

    return mongoClient;
});



// Register MongoDB database instance
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});



var app = builder.Build();
app.Logger.LogInformation("Using connection string: {ConnectionString}", connectionString);

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CodeBaseDbContext>();
        await context.Database.EnsureCreatedAsync();
        app.Logger.LogInformation("Database migrations applied successfully");
        // Check if seeding is needed based on environment
        var sqlDirectory = "./SqlScripts";
        var fileList = new List<string>
        {
            "CodeBases"
        };
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

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

await app.RunAsync();


/// <summary>
/// This class is used to start the API,
/// Partial class is used to add the entry point for CustomWebApplicationFactory
/// </summary>
public abstract partial class Program;