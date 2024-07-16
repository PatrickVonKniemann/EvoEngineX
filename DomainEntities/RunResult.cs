using Newtonsoft.Json.Bson;

namespace DomainEntities;

public class RunResult
{
    public Guid Id { get; set; }
    public byte[]? File { get; set; }
    public required BsonDataObjectId ObjectRefId { get; set; }
}