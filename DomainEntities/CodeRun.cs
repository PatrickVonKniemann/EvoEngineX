namespace DomainEntities;

public class CodeRun
{
    public Guid Id { get; set; }
    public Guid CodeBaseId { get; set; }
    // Navigation property for CodeBase
    public required CodeBase CodeBase { get; set; }
    public RunStatus Status { get; set; } = RunStatus.Ready;
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}

