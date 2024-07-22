namespace ExternalDomainEntities.CodeBaseDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeBaseListByUserIdResponse
{
    public List<ReadCodeBaseListResponseItem>? CodeBaseListResponseItems { get; set; }
}