using MongoDB.Bson;

namespace DomainEntities;

public class RunResult
{
    public Guid Id { get; set; }
    public byte[]? File { get; set; }
    public required ObjectId ObjectRefId { get; set; }
}