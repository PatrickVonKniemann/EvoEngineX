using CodeBaseService.Infrastructure;
using Common;
using Generics.Enums;

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
                Code = "Hello wordls",
                Name = "Testing Code",
                SupportedPlatform = SupportedPlatformType.Csharp,
                Valid = true,
                UserId = MockData.MockId
            },
            new DomainEntities.CodeBase
            {
                Id = MockData.MockIdUpdate,
                Code = "Hello wordls",
                Name = "Testing Code",
                SupportedPlatform = SupportedPlatformType.Java,
                Valid = false,
                UserId = new Guid("222e4567-e89b-12d3-a456-426614174000")
            },
            new DomainEntities.CodeBase
            {
                Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                Code = "Hello wordls",
                Name = "Testing Code",
                SupportedPlatform = SupportedPlatformType.Matlab,
                Valid = false,
                UserId = new Guid("333e4567-e89b-12d3-a456-426614174000")
            },
            new DomainEntities.CodeBase
            {
                Id = new Guid("555e4567-e89b-12d3-a456-426614174000"),
                Code = "Hello wordls",
                Name = "Testing Code",
                SupportedPlatform = SupportedPlatformType.Java,
                Valid = true,
                UserId = new Guid("444e4567-e89b-12d3-a456-426614174000")
            });
    }
}