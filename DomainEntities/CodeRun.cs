using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DomainEntities;

public class CodeRun
{
    [Key] [Required] public Guid Id { get; set; }

    [Required] public Guid CodeBaseId { get; set; }

    // Navigation property for CodeBase
    [Required] public required CodeBase CodeBase { get; set; }
    [Required] public RunStatus Status { get; set; } = RunStatus.Ready;
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}