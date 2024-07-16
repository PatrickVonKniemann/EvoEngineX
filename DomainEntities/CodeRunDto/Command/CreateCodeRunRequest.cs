namespace DomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodeRunRequest
{
    public Guid Id { get; set; }
    public Guid CodeBaseId { get; set; }
}