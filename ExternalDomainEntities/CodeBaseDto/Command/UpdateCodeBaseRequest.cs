﻿using Generics.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class UpdateCodeBaseRequest
{
    [FromRoute]
    public Guid Id { get; set; }
    public RunStatus? Status { get; set; }
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}