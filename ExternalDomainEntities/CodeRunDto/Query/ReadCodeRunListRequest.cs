using Generics.Pagination;

namespace ExternalDomainEntities.CodeRunDto.Query;

public class ReadCodeRunListRequest
{
    public PaginationQuery? PaginationQuery { get; set; }
}