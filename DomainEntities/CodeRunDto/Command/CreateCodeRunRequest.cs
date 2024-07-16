﻿namespace DomainEntities.CodeRunDtos.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class CreateCodeRunRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
        
    public string Language { get; set; } = string.Empty;
}