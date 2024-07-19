using System.ComponentModel.DataAnnotations;

namespace DomainEntities;

public class CodeBase
{
    [Key] public Guid Id { get; set; }
    public string? Code { get; set; }
    [Required] public Guid UserId { get; set; }
    // Navigation property for CodeRuns
    public List<Guid>? CodeRunIds { get; set; }

}