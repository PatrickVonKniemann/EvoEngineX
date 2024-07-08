namespace DomainEntities.Users.Request;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateUserRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
        
    public string Language { get; set; } = string.Empty;
}