using AutoMapper;
using Generics.BaseEntities;
using CodeRunService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Query;

namespace CodeRunService.Application.Services
{
    public class CodeRunQueryService(
        IMapper mapper,
        ICodeRunRepository codeRunRepository,
        ILogger<CodeRunQueryService> logger)
        : BaseQueryService<CodeRun, ReadCodeRunResponse, ReadCodeRunListResponseItem, ReadCodeRunListResponse>(mapper,
            codeRunRepository, logger), ICodeRunQueryService
    {
        private readonly IMapper _mapper = mapper;

        public async Task<ReadCodeRunListByCodeBaseIdResponse> GetAllByCodeBaseIdAsync(Guid codeBaseId)
        {
            logger.LogInformation($"{nameof(CodeRunQueryService)} {nameof(GetAllByCodeBaseIdAsync)}");
            var codeRuns = await codeRunRepository.GetAllByCodeBaseIdAsync(codeBaseId);
            return new ReadCodeRunListByCodeBaseIdResponse
            {
                CodeRunListResponseItems = _mapper.Map<List<ReadCodeRunListResponseItem>>(codeRuns)
            };
        }
    }
}