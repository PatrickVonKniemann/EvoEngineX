using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Common;

public class CustomWebApplicationFactory<TStartup, TDbContext>(Action<TDbContext> seedAction)
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

                // Add a new DbContext using an in-memory database for testing.
                services.AddDbContext<TDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts.
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TDbContext>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Log before seeding
                    Console.WriteLine("Seeding the database...");
                    // Seed the database with mock data.
                    seedAction(db);
                    db.SaveChanges();
                    // Log after seeding
                    Console.WriteLine("Seeding completed.");
                }
                catch (Exception ex)
                {
                    // Log any errors during seeding.
                    Console.WriteLine($"An error occurred seeding the database. Error: {ex.Message}");
                }
            });
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Override to configure the web host for testing
        builder.UseEnvironment("Development"); // Set the environment to Development or Testing
    }
}