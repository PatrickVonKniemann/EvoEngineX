using ExternalDomainEntities.CodeBaseDto.Query;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodebaseService.Application.Services;

public interface ICodeBaseQueryService : IGenericEntityQueryService<ReadCodeBaseResponse, ReadCodeBaseListResponse>
{
    Task<ReadCodeBaseListByUserIdResponse> GetAllByUserIdAsync(Guid userId, PaginationQuery? paginationQuery);
}