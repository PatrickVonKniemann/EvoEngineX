namespace DomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodebaseResponse
{
    public Guid Id { get; set; }
    public Guid CodeBaseId { get; set; }
}