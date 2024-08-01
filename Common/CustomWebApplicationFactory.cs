using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common
{
    public class CustomWebApplicationFactory<TStartup, TDbContext>(
        string databaseName,
        List<string> sqlFileList,
        string sqlDirectory)
        : WebApplicationFactory<TStartup>
        where TStartup : class
        where TDbContext : DbContext
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration.
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<TDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add a new DbContext using the PostgreSQL database for testing.
                    services.AddDbContext<TDbContext>(options =>
                    {
                        var connectionString = $"Host=localhost;port=5434;Database={databaseName};Username=kolenpat;Password=sa";
                        options.UseNpgsql(connectionString);
                    });

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database contexts.
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<TDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreatedAsync().GetAwaiter().GetResult();

                    var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup, TDbContext>>>();
                    SeedDatabase(logger).GetAwaiter().GetResult();
                    
                    // Seed the database with data from SQL files.
                    // var logger = scopedServices
                    //     .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup, TDbContext>>>();

                    // if (DbHelper.IsDatabaseEmptyAsync(db, logger).GetAwaiter().GetResult())
                    // {
                    //     SeedDatabase(logger).GetAwaiter().GetResult();
                    // }
                });
            });

            return base.CreateHost(builder);
        }

        private async Task SeedDatabase(ILogger<CustomWebApplicationFactory<TStartup, TDbContext>> logger)
        {
            var connectionString = $"Host=localhost;port=5434;Database={databaseName};Username=kolenpat;Password=sa";
            await DbHelper.RunSeedSqlFileAsync(sqlDirectory, logger, connectionString, sqlFileList);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Override to configure the web host for testing
            builder.UseEnvironment("IntegrationTesting"); // Set the environment to Development or Testing
        }
    }
}
