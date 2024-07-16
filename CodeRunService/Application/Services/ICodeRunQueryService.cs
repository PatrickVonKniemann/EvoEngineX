using DomainEntities.CodeRunDto.Query;
using Generics.BaseEntities;

namespace CodeRunService.Application.Services;

public interface ICodeRunQueryService : IGenericEntityQueryService<ReadCodeRunResponse, ReadCodeRunListResponse>
{
    
}