using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.CodeRunDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}