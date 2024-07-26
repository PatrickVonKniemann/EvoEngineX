using ExternalDomainEntities.CodeBaseDto.Query;
using Generics.BaseEntities;

namespace CodebaseService.Application.Services;

public interface ICodeBaseQueryService : IGenericEntityQueryService<ReadCodeBaseResponse, ReadCodeBaseListResponse>
{
    Task<ReadCodeBaseListByUserIdResponse> GetAllByUserIdAsync(Guid userId);
}