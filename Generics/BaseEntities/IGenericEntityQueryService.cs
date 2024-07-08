using Generics.Pagination;

namespace Generics.BaseEntities;

/// <summary>
/// Generic service for managing entity queries.
/// </summary>
/// <typeparam name="TListResponse"></typeparam>
/// <typeparam name="TReadResponse"></typeparam>
public interface IGenericEntityQueryService<out TReadResponse, out TListResponse>
{
    /// <summary>
    /// Get all
    /// </summary>
    /// <returns></returns>
    TListResponse? GetAll(PaginationQuery? paginationQuery);

    /// <summary>
    /// Get by id
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    TReadResponse GetById(Guid entityId);
}