using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;

namespace Generics.Enums;

public class RunResult
{
    [Key] public Guid Id { get; set; }
    public byte[]? File { get; set; }
    [NotMapped]
    public required ObjectId ObjectRefId { get; set; }
}