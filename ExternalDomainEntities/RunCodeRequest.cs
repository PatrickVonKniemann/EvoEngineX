namespace ExternalDomainEntities;

/// <summary>
///  User data transfer object.
/// </summary>
public class RunCodeRequest
{
    public required Guid CodeBaseId { get; init; }
    public required string Code { get; set; }
}