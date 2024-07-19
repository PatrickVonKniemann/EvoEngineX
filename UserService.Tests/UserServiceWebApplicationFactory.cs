using DomainEntities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Infrastructure;

namespace UserService.Tests;

public class UserServiceWebAplicatonFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureWebHost(webHostBuilder =>
        {
            webHostBuilder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<UserDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a new DbContext using an in-memory database for testing.
                services.AddDbContext<UserDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts.
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<UserDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with mock data.
                        SeedData(db);
                    }
                    catch (Exception ex)
                    {
                        // Log any errors during seeding.
                        Console.WriteLine($"An error occurred seeding the database. Error: {ex.Message}");
                    }
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

    private void SeedData(UserDbContext context)
    {
        // Add mock data to the in-memory database
        context.Users.AddRange(
            new User
            {
                Id = MockData.MockId,
                UserName = "john_doe",
                Email = "john.doe@example.com",
                Name = "John Doe",
                Language = "English",
                Password = "Pass1"
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "jane_smith",
                Email = "jane.smith@example.com",
                Name = "Jane Smith",
                Language = "English",
                Password = "Pass2"
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "maria_garcia",
                Email = "maria.garcia@example.com",
                Name = "Maria Garcia",
                Language = "Spanish",
                Password = "Pass3"
            },
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "maria_garcia2",
                Email = "maria2.garcia@example.com",
                Name = "Maria2 Garcia",
                Language = "Spanish",
                Password = "Pass3"
            }
        );
    }
}