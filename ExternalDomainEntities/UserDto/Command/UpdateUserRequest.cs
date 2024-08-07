﻿namespace ExternalDomainEntities.UserDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class UpdateUserRequest
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; } = string.Empty;

    public string? UserName { get; set; } = string.Empty;
    
    public string? Password { get; set; } = string.Empty;

    public string? Language { get; set; } = string.Empty;
}