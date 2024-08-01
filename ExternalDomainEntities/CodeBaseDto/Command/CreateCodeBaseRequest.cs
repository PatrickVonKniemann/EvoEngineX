using Generics.Enums;

namespace ExternalDomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodeBaseRequest
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public SupportedPlatformType SupportedPlatform { get; set; }
    public bool Valid { get; set; } 

}