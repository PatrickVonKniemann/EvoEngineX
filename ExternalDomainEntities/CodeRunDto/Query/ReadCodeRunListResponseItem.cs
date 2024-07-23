using Generics.Enums;

namespace ExternalDomainEntities.CodeRunDto.Query;

/// <summary>
/// Item of user list
/// </summary>
public class ReadCodeRunListResponseItem
{
    public Guid Id { get; set; }
    public Guid CodeBaseId { get; set; }
    public RunStatus Status { get; set; }
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}