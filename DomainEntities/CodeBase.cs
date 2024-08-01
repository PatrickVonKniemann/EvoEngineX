using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Generics.Enums;

namespace DomainEntities;

public class CodeBase
{
    [Key] public Guid Id { get; set; }
    public required string Name { get; set; }
    public required SupportedPlatformType SupportedPlatform { get; set; }
    public required bool Valid { get; set; }
    public string? Code { get; set; }
    [NotMapped] public Dictionary<string, string>? Parameters { get; set; }
    public required Guid UserId { get; set; }
    // public List<Guid>? Collaborators { get; set; } 
}