using Generics.Enums;

namespace ExternalDomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class UpdateCodeBaseRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public SupportedPlatformType? SupportedPlatform { get; set; }
    public bool? Valid { get; set; }
    public string? Code { get; set; }
    public Dictionary<string, string>? Parameters { get; set; }
}