using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeBaseDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodebaseRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}