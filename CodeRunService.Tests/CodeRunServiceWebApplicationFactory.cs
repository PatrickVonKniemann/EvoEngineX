using CodeRunService.Tests;
using Common;
using CodeRunService.Infrastructure;
using DomainEntities;
using Generics.Enums;
using MongoDB.Bson;

namespace CodeRunService.Tests;

public class CodeRunServiceWebApplicationFactory<TStartup>()
    : CustomWebApplicationFactory<TStartup, CodeRunDbContext>(SeedData)
    where TStartup : class
{
    private static void SeedData(CodeRunDbContext context)
    {
        context.CodeRuns.AddRange(
            new CodeRun
            {
                Id = MockData.MockId,
                CodeBaseId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
                Status = RunStatus.Ready,
                RunStart = DateTime.UtcNow,
                RunFinish = null,
                Results = null
            },
            new CodeRun
            {
                Id = MockData.MockIdUpdate,
                CodeBaseId = new Guid("333e4567-e89b-12d3-a456-426614174000"),
                Status = RunStatus.Running,
                RunStart = DateTime.UtcNow.AddHours(-1),
                RunFinish = null,
                Results = null
            },
            new CodeRun
            {
                Id = Guid.NewGuid(),
                CodeBaseId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                Status = RunStatus.Ready,
                RunStart = DateTime.UtcNow.AddHours(-2),
                RunFinish = DateTime.UtcNow.AddHours(-1),
                Results = new RunResult
                {
                    Id = Guid.NewGuid(),
                    File = null,
                    ObjectRefId = ObjectId.GenerateNewId()
                }
            },
            new CodeRun
            {
                Id = Guid.NewGuid(),
                CodeBaseId = new Guid("555e4567-e89b-12d3-a456-426614174000"),
                Status = RunStatus.Ready,
                RunStart = DateTime.UtcNow.AddHours(-3),
                RunFinish = DateTime.UtcNow.AddHours(-2),
                Results = new RunResult
                {
                    Id = Guid.NewGuid(),
                    File = null,
                    ObjectRefId = ObjectId.GenerateNewId()
                }
            });


        // Ensure data is saved
        context.SaveChanges();
    }
}