using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.Users.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadUserRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}