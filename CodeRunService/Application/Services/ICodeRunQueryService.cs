using ExternalDomainEntities.CodeRunDto.Query;
using Generics.BaseEntities;

namespace CodeRunService.Application.Services;

public interface ICodeRunQueryService : IGenericEntityQueryService<ReadCodeRunResponse, ReadCodeRunListResponse>
{
    Task<ReadCodeRunListByCodeBaseIdResponse> GetAllByCodeBaseIdAsync(Guid codeBaseId);
}