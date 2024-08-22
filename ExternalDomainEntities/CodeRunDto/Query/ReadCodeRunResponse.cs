using Generics.Enums;

namespace ExternalDomainEntities.CodeRunDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeRunResponse
{
    public required Guid Id { get; set; }
    public required Guid CodeBaseId { get; set; }
    public required string Code { get; set; } 
    public required RunStatus Status { get; set; } 
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}