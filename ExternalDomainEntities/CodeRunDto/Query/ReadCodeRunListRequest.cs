using Generics.Pagination;

namespace ExternalDomainEntities.CodeRunDto.Query;

public class ReadCodeRunListRequest
{
    public required PaginationQuery? PaginationQuery { get; set; }
}