using Microsoft.AspNetCore.Mvc;

namespace DomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteCodeBaseRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}