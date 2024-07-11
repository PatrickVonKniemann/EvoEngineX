namespace Generics.BaseEntities;

/// <summary>
/// Generic service for managing entity commands.
/// </summary>
/// <typeparam name="TEntityCreateRequest"></typeparam>
/// <typeparam name="TEntityUpdateRequest"></typeparam>
/// <typeparam name="TEntityUpdateReadResponse"></typeparam>
/// <typeparam name="TEntityCreateReadResponse"></typeparam>
public interface IGenericCommandService<in TEntityCreateRequest, in TEntityUpdateRequest, TEntityCreateReadResponse,
    TEntityUpdateReadResponse>
{
    Task<TEntityCreateReadResponse> AddAsync(TEntityCreateRequest entityCreateDto);
    Task<TEntityUpdateReadResponse> UpdateAsync(Guid entityId, TEntityUpdateRequest entityUpdateDto);
    Task DeleteAsync(Guid entityId);
}