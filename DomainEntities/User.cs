using System.ComponentModel.DataAnnotations;

namespace DomainEntities;

public class User : CommonEntityObject
{
    [Key] public Guid Id { get; set; }

    [Required] [MaxLength(50)] public required string UserName { get; set; }

    [Required] [MaxLength(255)] public required string Password { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public required string Email { get; set; }

    [Required] [MaxLength(100)] public required string Name { get; set; }

    [Required] [MaxLength(10)] public required string Language { get; set; }
}