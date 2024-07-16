using Generics.Pagination;

namespace DomainEntities.CodeRunDto.Query;

public class ReadCodeRunListRequest
{
    public required PaginationQuery? PaginationQuery { get; set; }
}