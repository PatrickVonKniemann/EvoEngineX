using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeBaseDto.Query;
using Generics.BaseEntities;

namespace CodeBaseService.Application.Services;

public class CodeBaseQueryService(IMapper mapper, ICodeBaseRepository codeBaseRepository, ILogger<CodeBaseQueryService> logger)
    : BaseQueryService<CodeBase, ReadCodeBaseResponse, ReadCodeBaseListResponseItem, ReadCodeBaseListResponse>(mapper, codeBaseRepository,
        logger), ICodeBaseQueryService
{
    // Any additional methods specific to CodeBaseQueryService can go here
    public async Task<ReadCodeBaseListByUserIdResponse> GetAllByUserIdAsync(Guid userId)
    {
        logger.LogInformation($"{nameof(CodeBaseQueryService)} {nameof(GetAllByUserIdAsync)}");
        var codebases = await codeBaseRepository.GetAllByUserIdAsync(userId);
        return new ReadCodeBaseListByUserIdResponse
        {
            CodeBaseListResponseItems = mapper.Map<List<ReadCodeBaseListResponseItem>>(codebases)
        };
    }
}


        


