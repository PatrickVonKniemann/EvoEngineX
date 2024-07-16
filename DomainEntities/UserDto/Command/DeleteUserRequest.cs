﻿using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.UserDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteUserRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}