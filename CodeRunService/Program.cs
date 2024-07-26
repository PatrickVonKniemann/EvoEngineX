using CodeRunService.Application.Services;
using CodeRunService.Infrastructure;
using CodeRunService.Infrastructure.Database;
using Common;
using DomainEntities;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddLogging(); // Ensure logging is added first

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<ICodeRunCommandService, CodeRunCommandService>();
builder.Services.AddScoped<ICodeRunQueryService, CodeRunQueryService>();
builder.Services.AddScoped<ICodeRunRepository, CodeRunRepository>();

builder.Services.AddAutoMapper(cg => cg.AddProfile(new CodeRunProfile()));

var connectionString = builder.Configuration.GetConnectionString("CodeRunDatabase");
connectionString = connectionString
    .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
    .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "CodeRunDb")
    .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "kolenpat")
    .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa");


builder.Services.AddDbContext<CodeRunDbContext>(options =>
    options.UseNpgsql(connectionString));


var app = builder.Build();
app.Logger.LogInformation("Using connection string: {ConnectionString}", connectionString);


// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CodeRunDbContext>();
        await context.Database.EnsureCreatedAsync();
        app.Logger.LogInformation("Database migrations applied successfully");
        await DbHelper.RunSeedSqlFileAsync(app.Logger, connectionString, new List<string>
        {
            "RunResults",
            "CodeRuns"
        });
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

await app.RunAsync();

/// <summary>
/// This class is used to start the API,
/// Partial class is used to add the entry point for CustomWebApplicationFactory
/// </summary>
public partial class Program
{
}