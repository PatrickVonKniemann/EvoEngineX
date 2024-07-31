using System.ComponentModel.DataAnnotations;
using Generics.Enums;

namespace DomainEntities;

public class CodeBase
{
    [Key] public Guid Id { get; set; }
    public required string Name { get; set; }
    public required SupportedPlatformType SupportedPlatform { get; set; }
    public required bool Valid { get; set; } = false;
    public string? Code { get; set; }
    // public List<Guid>? Collaborators { get; set; } 
    [Required] public Guid UserId { get; set; }
}