namespace ExternalDomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodeRunRequest
{
    public required Guid CodeBaseId { get; set; }
}