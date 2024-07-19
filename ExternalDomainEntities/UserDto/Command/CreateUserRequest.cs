namespace ExternalDomainEntities.UserDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateUserRequest
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Language { get; set; }
    public required string Password { get; set; }
}