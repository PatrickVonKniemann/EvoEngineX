using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.Users.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteUserRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}