using System.ComponentModel.DataAnnotations;
using Generics.Enums;

namespace DomainEntities;

public class CodeRun
{
    [Key] public Guid Id { get; set; }
    public required Guid CodeBaseId { get; init; }
    public required RunStatus Status { get; set; }
    public required DateTime RunStart { get; set; }
    public required DateTime? RunFinish { get; set; }
    public required RunResult? Results { get; set; }
}