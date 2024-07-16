using System.Diagnostics.CodeAnalysis;

namespace DomainEntities;

public class CodeRun
{
    public CodeRun()
    {
    }

    [SetsRequiredMembers]
    public CodeRun(Guid id, Guid codebaseId, RunStatus status, DateTime? runStart, DateTime? runFinish, RunResult? results, Codebase? codebase)
    {
        Id = id;
        CodebaseId = codebaseId;
        Status = status;
        RunStart = runStart;
        RunFinish = runFinish;
        Results = results;
        Codebase = codebase;
    }

    public Guid Id { get; set; }
    public Guid CodebaseId { get; set; }
    // Navigation property for Codebase TODO remove this after adding dbcontext
    public required Codebase? Codebase { get; set; }
    public RunStatus Status { get; set; } = RunStatus.Ready;
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}

