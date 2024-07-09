namespace DomainEntities.Users.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class UpdateUserRequest
{
    public string? Email { get; set; }

    public string Name { get; set; } = string.Empty;
        
    public string Language { get; set; } = string.Empty;
}