namespace ExternalDomainEntities.CodeBaseDto.Query;

/// <summary>
/// Item of user list
/// </summary>
public class ReadCodeBaseListResponseItem
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required bool Valid { get; set; } = false;
    public string? Code { get; set; }
    // public List<Guid>? Collaborators { get; set; } 
}