using Generics.Pagination;

namespace ExternalDomainEntities.CodeBaseDto.Query;

public class ReadCodeBaseListRequest
{
    public required PaginationQuery? PaginationQuery { get; set; }
}