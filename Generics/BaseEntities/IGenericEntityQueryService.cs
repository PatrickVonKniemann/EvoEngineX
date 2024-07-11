using Generics.Pagination;

namespace Generics.BaseEntities;

/// <summary>
/// Generic service for managing entity queries.
/// </summary>
/// <typeparam name="TListResponse"></typeparam>
/// <typeparam name="TReadResponse"></typeparam>
public interface IGenericEntityQueryService<TReadResponse, TListResponse>
{
    /// <summary>
    /// Get all
    /// </summary>
    /// <returns></returns>
    Task<TListResponse> GetAllAsync(PaginationQuery paginationQuery);

    /// <summary>
    /// Get by id
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    Task<TReadResponse> GetByIdAsync(Guid entityId);
}