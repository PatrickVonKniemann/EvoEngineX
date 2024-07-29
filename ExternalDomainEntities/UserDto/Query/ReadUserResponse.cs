namespace ExternalDomainEntities.UserDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadUserResponse
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;
}