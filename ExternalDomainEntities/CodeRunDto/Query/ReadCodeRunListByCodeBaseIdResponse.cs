namespace ExternalDomainEntities.CodeRunDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeRunListByCodeBaseIdResponse
{
    public List<ReadCodeRunListResponseItem>? CodeRunListResponseItems { get; set; }
}