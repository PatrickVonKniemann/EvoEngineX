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