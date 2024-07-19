using DomainEntities;
using Generics.Pagination;

namespace UserService.Tests;

public static class MockData
{
    public static readonly Guid MockId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");

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
}