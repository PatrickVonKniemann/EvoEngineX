namespace ExternalDomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodeBaseRequest
{
    public required Guid UserId { get; set; }
}