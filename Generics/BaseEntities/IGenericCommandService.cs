namespace Generics.BaseEntities;

/// <summary>
/// Generic service for managing entity commands.
/// </summary>
/// <typeparam name="TEntityCreateRequest"></typeparam>
/// <typeparam name="TEntityUpdateRequest"></typeparam>
/// <typeparam name="TEntityUpdateReadResponse"></typeparam>
/// <typeparam name="TEntityCreateReadResponse"></typeparam>
public interface IGenericCommandService<in TEntityCreateRequest, in TEntityUpdateRequest, out TEntityCreateReadResponse,
    out TEntityUpdateReadResponse>
{
    /// <summary>
    /// Add entity
    /// </summary>
    /// <returns></returns>
    TEntityCreateReadResponse Add(TEntityCreateRequest entityCreateDto);

    /// <summary>
    /// Update entity by id
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="entityUpdateDto"></param>
    /// <returns></returns>
    TEntityUpdateReadResponse Update(Guid entityId, TEntityUpdateRequest entityUpdateDto);

    /// <summary>
    /// Delete entity by id
    /// </summary>
    /// <param name="entityId"></param>
    void Delete(Guid entityId);
}