using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeRunDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}