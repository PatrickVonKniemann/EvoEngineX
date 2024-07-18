namespace DomainEntities.CodeRunDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeRunResponse
{
    public Guid Id { get; set; }
    public Guid CodeBaseId { get; set; }
    // Navigation property for CodeBase
    public required CodeBase CodeBase { get; set; }
    public RunStatus Status { get; set; } 
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}