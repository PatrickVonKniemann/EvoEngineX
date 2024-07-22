using Generics.Pagination;

namespace ExternalDomainEntities.UserDto.Query;

public class ReadUserListRequest
{
    public PaginationQuery? PaginationQuery { get; set; }
}