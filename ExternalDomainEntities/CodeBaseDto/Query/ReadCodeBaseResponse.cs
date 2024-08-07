﻿using Generics.Enums;

namespace ExternalDomainEntities.CodeBaseDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeBaseResponse
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required SupportedPlatformType SupportedPlatform { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
}