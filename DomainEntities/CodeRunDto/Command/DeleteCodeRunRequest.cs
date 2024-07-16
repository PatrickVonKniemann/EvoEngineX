using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}