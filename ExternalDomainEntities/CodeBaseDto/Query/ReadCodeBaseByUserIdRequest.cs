using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.CodeBaseDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeBaseListByUserIdRequest
{
    [FromRoute]
    public Guid UserId { get; set; }
}