using ExternalDomainEntities.CodeRunDto.Query;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodeRunService.Application.Services;

public interface ICodeRunQueryService : IGenericEntityQueryService<ReadCodeRunResponse, ReadCodeRunListResponse>
{
    Task<ReadCodeRunListByCodeBaseIdResponse> GetAllByCodeBaseIdAsync(Guid codeBaseId,
        PaginationQuery? paginationQuery);
}