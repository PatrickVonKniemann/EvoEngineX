using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class UpdateCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
    
}