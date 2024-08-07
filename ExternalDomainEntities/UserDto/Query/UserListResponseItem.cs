namespace ExternalDomainEntities.UserDto.Query;

/// <summary>
/// Item of user list
/// </summary>
public class UserListResponseItem
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;


}