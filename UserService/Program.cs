using Common;
using DomainEntities;
using FastEndpoints;
using FastEndpoints.Swagger;
using Helpers;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Services;
using UserService.Infrastructure;
using UserService.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// Environment-specific configuration
var environment = builder.Environment.EnvironmentName;

// Configure logging explicitly based on environment
ConfigureLogging(builder.Logging, environment);

// Add services to the container
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

app.Logger.LogInformation("Using connection string: {ConnectionString}", GetConnectionString(builder.Configuration));

// Apply database migrations and seeding
await ApplyDatabaseMigrationsAndSeedingAsync(app);

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

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddFastEndpoints()
        .SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "UserService API";
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

    services.AddScoped<IUserCommandService, UserCommandService>();
    services.AddScoped<IUserQueryService, UserQueryService>();
    services.AddScoped<IUserRepository, UserRepository>();

    services.AddAutoMapper(cg => cg.AddProfile(new UserProfile()));

    var connectionString = GetConnectionString(configuration);
    services.AddDbContext<UserDbContext>(options =>
        options.UseNpgsql(connectionString));
}

string GetConnectionString(IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("UserDatabase");

    return connectionString?.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost")
               .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "5433")
               .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "UserServiceDb")
               .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "kolenpat")
               .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "sa") ??
           throw new InvalidOperationException("No connection string found");
}

async Task ApplyDatabaseMigrationsAndSeedingAsync(WebApplication appRuntime)
{
    using var scope = appRuntime.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<UserDbContext>();
        await context.Database.EnsureCreatedAsync();
        appRuntime.Logger.LogInformation("Database migrations applied successfully");

        var fileList = new List<string> { "Users" };
        var sqlDirectory = "./SqlScripts";
        await DbHelper.RunSeedSqlFileAsync(sqlDirectory, appRuntime.Logger,
            GetConnectionString(appRuntime.Configuration), fileList);
    }
    catch (Exception ex)
    {
        appRuntime.Logger.LogError(ex, "An error occurred while applying migrations");
    }
}

void ConfigureMiddleware(WebApplication appRuntime)
{
    if (appRuntime.Environment.IsDevelopment())
    {
        appRuntime.UseDeveloperExceptionPage();
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