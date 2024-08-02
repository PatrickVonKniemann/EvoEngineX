using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeBaseDto.Query;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodeBaseService.Application.Services;

public class CodeBaseQueryService(
    IMapper mapper,
    ICodeBaseRepository codeBaseRepository,
    ILogger<CodeBaseQueryService> logger)
    : BaseQueryService<CodeBase, ReadCodeBaseResponse, ReadCodeBaseListResponseItem, ReadCodeBaseListResponse>(mapper,
        codeBaseRepository,
        logger), ICodeBaseQueryService
{
    private readonly IMapper _mapper = mapper;

    // Any additional methods specific to CodeBaseQueryService can go here
    public async Task<ReadCodeBaseListByUserIdResponse> GetAllByUserIdAsync(Guid userId,
        PaginationQuery? paginationQuery)
    {
        logger.LogInformation($"{nameof(CodeBaseQueryService)} {nameof(GetAllByUserIdAsync)}");
        List<CodeBase> codebases;
        if (paginationQuery != null)
        {
            codebases = await codeBaseRepository.GetAllByUserIdAsync(userId, paginationQuery);
        }
        else
        {
            codebases = await codeBaseRepository.GetAllByUserIdAsync(userId);
        }

        return new ReadCodeBaseListByUserIdResponse
        {
            Items =
            {
                Values = _mapper.Map<List<ReadCodeBaseListResponseItem>>(codebases)
            }
        };
    }
}