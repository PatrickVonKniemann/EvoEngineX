namespace ExternalDomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodeRunResponse
{
    public Guid Id { get; set; }
    public Guid CodeBaseId { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
}