namespace Generics.BaseEntities;

public interface IGenericEntityCommandService<in TCreateRequest, TCreateResponse, in TUpdateRequest, TUpdateResponse>
{
    Task<TCreateResponse> AddAsync(TCreateRequest entityRequest);
    Task<TUpdateResponse> UpdateAsync(Guid entityId, TUpdateRequest entityRequest);
    Task DeleteAsync(Guid entityId);
}