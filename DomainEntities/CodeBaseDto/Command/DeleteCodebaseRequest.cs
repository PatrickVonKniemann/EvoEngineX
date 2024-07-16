using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteCodebaseRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}