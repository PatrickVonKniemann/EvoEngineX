using CodeBaseService.Infrastructure;
using Common;

namespace CodeBase.Tests;

public class CodeBaseServiceWebApplicationFactory<TStartup>()
    : CustomWebApplicationFactory<TStartup, CodeBaseDbContext>(SeedData)
    where TStartup : class
{
    private static void SeedData(CodeBaseDbContext context)
    {
        context.CodeBases.AddRange(
            new DomainEntities.CodeBase
            {
                Id = MockData.MockId,
                Code = "Sample code 1",
                UserId = new Guid("111e4567-e89b-12d3-a456-426614174000")
            },
            new DomainEntities.CodeBase
            {
                Id = MockData.MockIdUpdate,
                Code = "Sample code 2",
                UserId = new Guid("222e4567-e89b-12d3-a456-426614174000")
            },
            new DomainEntities.CodeBase
            {
                Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                Code = "Sample code 3",
                UserId = new Guid("333e4567-e89b-12d3-a456-426614174000")
            },
            new DomainEntities.CodeBase
            {
                Id = new Guid("555e4567-e89b-12d3-a456-426614174000"),
                Code = "Sample code 4",
                UserId = new Guid("444e4567-e89b-12d3-a456-426614174000")
            });
    }
}