using Generics.Pagination;

namespace ExternalDomainEntities.CodeBaseDto.Query;

public class ReadCodeBaseListRequest
{
    public PaginationQuery? PaginationQuery { get; set; }
}