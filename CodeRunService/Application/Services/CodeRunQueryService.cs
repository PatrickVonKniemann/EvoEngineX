using AutoMapper;
using Generics.BaseEntities;
using CodeRunService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Query;
using Generics.Pagination;

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

        public async Task<ReadCodeRunListByCodeBaseIdResponse> GetAllByCodeBaseIdAsync(Guid codeBaseId,
            PaginationQuery? paginationQuery)
        {
            logger.LogInformation($"{nameof(CodeRunQueryService)} {nameof(GetAllByCodeBaseIdAsync)}");
            List<CodeRun> codeRuns;
            if (paginationQuery != null)
            {
                codeRuns = await codeRunRepository.GetAllByCodeBaseIdAsync(codeBaseId, paginationQuery);
                var itemsCount = await codeRunRepository.GetCountByCodeBaseId(codeBaseId);
                return new ReadCodeRunListByCodeBaseIdResponse
                {
                    Items =
                    {
                        Values = _mapper.Map<List<ReadCodeRunListResponseItem>>(codeRuns)
                    },
                    Pagination = new PaginationResponse
                    {
                        PageNumber = paginationQuery.PageNumber,
                        PageSize = paginationQuery.PageSize,
                        ItemsCount = itemsCount,
                        TotalPages = (int)Math.Ceiling((double)itemsCount / paginationQuery.PageSize)
                    }
                };
            }

            codeRuns = await codeRunRepository.GetAllByCodeBaseIdAsync(codeBaseId);
            return new ReadCodeRunListByCodeBaseIdResponse
            {
                Items =
                {
                    Values = _mapper.Map<List<ReadCodeRunListResponseItem>>(codeRuns)
                }
            };
        }
    }
}