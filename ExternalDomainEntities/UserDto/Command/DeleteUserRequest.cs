namespace ExternalDomainEntities.UserDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteUserRequest
{
    public Guid Id { get; set; }
}