namespace ExternalDomainEntities.CodeBaseDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeBaseResponse
{
    public required Guid Id { get; set; }
    public string? Code { get; set; }
}