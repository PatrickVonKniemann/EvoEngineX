namespace ExternalDomainEntities.CodeRunDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeRunFileResponse
{
    public required byte[]? File { get; set; }
}