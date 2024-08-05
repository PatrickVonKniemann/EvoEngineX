using Generics.Pagination;

namespace ExternalDomainEntities.CodeRunDto.Query;

public class ReadCodeRunListByCodeBaseIdRequest
{
    public Guid CodeBaseId { get; set; }
    public PaginationQuery? PaginationQuery { get; set; }
}