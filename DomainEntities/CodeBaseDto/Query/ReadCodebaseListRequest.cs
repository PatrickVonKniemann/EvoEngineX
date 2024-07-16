using Generics.Pagination;

namespace DomainEntities.CodeBaseDto.Query;

public class ReadCodebaseListRequest
{
    public required PaginationQuery? PaginationQuery { get; set; }
}