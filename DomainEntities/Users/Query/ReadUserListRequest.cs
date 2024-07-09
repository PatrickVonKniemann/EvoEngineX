using FastEndpoints;
using Generics.Pagination;

namespace DomainEntities.Users.Query;

public class ReadUserListRequest
{
    public required PaginationQuery PaginationQuery;
}