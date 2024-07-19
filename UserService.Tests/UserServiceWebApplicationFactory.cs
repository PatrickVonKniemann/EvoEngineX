using Common;
using UserService.Infrastructure;
using DomainEntities;

namespace UserService.Tests;

public class UserServiceWebApplicationFactory<TStartup> : CustomWebApplicationFactory<TStartup, UserDbContext>
    where TStartup : class
{
    public UserServiceWebApplicationFactory()
        : base(SeedData)
    {
    }

    private static void SeedData(UserDbContext context)
    {
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

        // Ensure data is saved
        context.SaveChanges();
    }
}