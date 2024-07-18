using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.UserDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadUserRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}