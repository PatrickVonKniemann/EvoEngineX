namespace ExternalDomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodeBaseResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CodeBaseId { get; set; }
}