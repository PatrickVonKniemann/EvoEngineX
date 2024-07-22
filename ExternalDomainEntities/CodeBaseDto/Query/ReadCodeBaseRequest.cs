﻿using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.CodeBaseDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeBaseRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}