using System.ComponentModel.DataAnnotations;

namespace DomainEntities;

public class User
{
    [Key] public Guid Id { get; set; }
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Language { get; set; } = string.Empty;
}