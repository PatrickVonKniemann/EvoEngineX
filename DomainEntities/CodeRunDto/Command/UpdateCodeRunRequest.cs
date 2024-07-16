﻿using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeRunDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class UpdateCodeRunRequest
{
    [FromRoute]
    public Guid Id { get; set; }
    public RunStatus? Status { get; set; }
    public DateTime? RunStart { get; set; }
    public DateTime? RunFinish { get; set; }
    public RunResult? Results { get; set; }
}