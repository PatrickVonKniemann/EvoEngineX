using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}