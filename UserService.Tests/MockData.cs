using DomainEntities;
using Generics.Pagination;

namespace UserService.Tests;

public static class MockData
{
    public static readonly Guid MockId = new Guid("123e4567-e89b-12d3-a456-426614174003");
    public static readonly Guid MockIdUpdate = Guid.Parse("123e4567-e89b-12d3-a456-426614174005");

    public static readonly User MockUser = new()
    {
        UserName = "jdoe",
        Email = "jdoe@example.com",
        Name = "John Doe",
        Language = "English",
        Password = "Password"
    };

    public static readonly PaginationQuery MockPaginationQuery = new()
    {
        PageNumber = 1,
        PageSize = 10
    };

    public static readonly User ExpectedUser = new()
    {
        Id = MockId,
        UserName = "john_doe",
        Email = "john.doe@example.com",
        Name = "John Doe",
        Language = "English",
        Password = "Pass1"
    };
}