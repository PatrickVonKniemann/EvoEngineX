using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeRunDtos.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class UpdateCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
    
    public string? Email { get; set; }

    public string Name { get; set; } = string.Empty;
        
    public string Language { get; set; } = string.Empty;
}