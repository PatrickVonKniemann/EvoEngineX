using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Generics.Enums;

public class RunResult
{
    [Key] public Guid Id { get; set; }
    public byte[]? File { get; set; }
    public required ObjectId ObjectRefId { get; set; }
}