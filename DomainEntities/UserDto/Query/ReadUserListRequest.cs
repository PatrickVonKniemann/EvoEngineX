using Generics.Pagination;

namespace DomainEntities.UserDto.Query;

public class ReadUserListRequest
{
    public required PaginationQuery? PaginationQuery { get; set; }
}