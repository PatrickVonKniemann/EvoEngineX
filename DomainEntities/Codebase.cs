namespace DomainEntities;

public class Codebase
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public Guid UserId { get; set; }
    // Navigation property for User
    public User User { get; set; }
    // Navigation property for CodeRuns
    public ICollection<CodeRun>? CodeRuns { get; set; }
}
