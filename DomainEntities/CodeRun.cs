namespace DomainEntities;

public class CodeRun
{
    public Guid Id { get; set; }
    public Guid CodeBaseId { get; set; }
    // Navigation property for CodeBase
    public CodeBase CodeBase { get; set; }
    public RunStatus Status { get; set; }
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public byte[] Results { get; set; }
}

