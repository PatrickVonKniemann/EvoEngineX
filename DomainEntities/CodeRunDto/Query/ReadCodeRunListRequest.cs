using Generics.Pagination;

namespace DomainEntities.CodeRunDtos.Query;

public class ReadCodeRunListRequest
{
    public required PaginationQuery? PaginationQuery { get; set; }
}