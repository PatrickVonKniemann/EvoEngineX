using DomainEntities.CodeBaseDto.Query;
using Generics.BaseEntities;

namespace CodebaseService.Application.Services;

public interface ICodeBaseQueryService : IGenericEntityQueryService<ReadCodebaseResponse, ReadCodebaseListResponse>
{
    
}