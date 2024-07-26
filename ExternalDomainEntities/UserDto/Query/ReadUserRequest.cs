using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.UserDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadUserRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}