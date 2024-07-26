using Microsoft.AspNetCore.Mvc;

namespace ExternalDomainEntities.CodeRunDto.Query;

public class ReadCodeRunListByCodeBaseIdRequest
{
    [FromRoute]
    public Guid CodeBaseId { get; set; }}