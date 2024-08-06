using Generics.Enums;

namespace ExternalDomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public record CreateCodeRunDetailRequest
{
    public required Guid CodeBaseId { get; set; }
    public required RunStatus Status { get; set; }
    public required DateTime RunStart { get; set; }
}