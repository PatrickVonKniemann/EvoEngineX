using System.ComponentModel.DataAnnotations;

namespace DomainEntities;

public class User
{
    [Key] public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Language { get; set; }
}