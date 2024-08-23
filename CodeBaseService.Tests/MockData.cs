using DomainEntities;
using Generics.Enums;
using Generics.Pagination;

namespace CodeBase.Tests;

public static class MockData
{
    public static readonly Guid MockId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000");
    public static readonly Guid MockIdUpdate = Guid.Parse("123e4567-e89b-12d3-a456-426614174005");

    public static readonly CodeRun MockCodeRun = new()
    {
        Id = MockId,
        Code = "Hellow world",
        CodeBaseId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
        Status = RunStatus.Ready,
        RunStart = DateTime.UtcNow,
        RunFinish = null,
        Results = null
    };

    public static readonly PaginationQuery MockPaginationQuery = new()
    {
        PageNumber = 1,
        PageSize = 10
    };

    public static readonly CodeRun ExpectedCodeRun = new()
    {
        Id = MockId,
        Code = "Hellow world",
        CodeBaseId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
        Status = RunStatus.Ready,
        RunStart = DateTime.UtcNow,
        RunFinish = null,
        Results = null
    };
}