using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeRunDtos.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}