using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.CodeBaseDto.Command;

/// <summary>
///  User data transfer object.
/// </summary>
public class DeleteCodeBaseRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}