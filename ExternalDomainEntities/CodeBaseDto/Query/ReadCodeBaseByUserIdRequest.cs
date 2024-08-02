using Generics.Pagination;

namespace ExternalDomainEntities.CodeBaseDto.Query;

/// <summary>
///  User data transfer object.
/// </summary>
public class ReadCodeBaseListByUserIdRequest
{
    public Guid UserId { get; set; }
    public PaginationQuery? PaginationQuery { get; set; }
}