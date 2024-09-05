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

// Add services to the container
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

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add SignalR services
builder.Services.AddSignalR();  // This line ensures SignalR is added to the DI container

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policyBuilder =>
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Register application services
builder.Services.AddScoped<ICodeRunCommandService, CodeRunCommandService>();
builder.Services.AddScoped<ICodeRunQueryService, CodeRunQueryService>();
builder.Services.AddScoped<ICodeRunRepository, CodeRunRepository>();
builder.Services.AddScoped<ICodeExecutionCommandService, CodeExecutionCommandService>();
builder.Services.AddScoped<ICodeValidationService, CodeValidationService>();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new CodeRunProfile()));

// Configure RabbitMQ settings from environment variables
var rabbitMqSettings = new
{
    Host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
    User = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "kolenpat",
    Pass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "sa",
    Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672")
};

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
    new ConnectionFactory
    {
        HostName = rabbitMqSettings.Host,
        UserName = rabbitMqSettings.User,
        Password = rabbitMqSettings.Pass,
        Port = rabbitMqSettings.Port
    });

builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddHostedService<CodeValidationConsumer>();
builder.Services.AddHostedService<CodeExecutionConsumer>();

// Setup database connection
var connectionString = builder.Configuration.GetConnectionString("CodeRunDatabase")?
    .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
    .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "5433")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "CodeRunDb")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "kolenpat")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa");

builder.Services.AddDbContext<CodeRunDbContext>(options =>
    options.UseNpgsql(connectionString));

// Get MongoDB connection settings from environment variables
var mongoConnectionString = $"mongodb://{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_USERNAME") ?? "kolenpat"}:{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_PASSWORD") ?? "sa"}@{Environment.GetEnvironmentVariable("MONGO_HOST") ?? "localhost"}:{Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27018"}/{Environment.GetEnvironmentVariable("MONGO_DB") ?? "evoenginex_db"}";
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

// Log connection strings
app.Logger.LogInformation("Using connection string: {ConnectionString}", connectionString);
app.Logger.LogInformation("RabbitMQ connection: Host={Host}, User={User}, Port={Port}", 
    rabbitMqSettings.Host, rabbitMqSettings.User, rabbitMqSettings.Port);

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CodeRunDbContext>();
        await context.Database.EnsureCreatedAsync();
        app.Logger.LogInformation("Database created successfully");

        // Check if seeding is needed based on environment
        var sqlDirectory = "./SqlScripts";
        var fileList = new List<string>
        {
            "CodeRuns"
        };
        await DbHelper.RunSeedSqlFileAsync(sqlDirectory, app.Logger, connectionString, fileList);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while applying migrations or seeding the database");
    }
}

// Use middleware and other configurations
app.UseMiddleware<ErrorHandlingMiddleware>(); // Custom error handling middleware
app.UseFastEndpoints();
app.UseSwaggerGen();

app.MapHub<CodeRunHub>("/codeRunHub");
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

await app.RunAsync();

/// <summary>
/// This class is used to start the API,
/// Partial class is used to add the entry point for CustomWebApplicationFactory
/// </summary>
public abstract partial class Program;
