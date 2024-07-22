using AutoMapper;
using Common.Exceptions;
using Generics.BaseEntities;
using Generics.Pagination;
using CodeRunService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Query;

namespace CodeRunService.Application.Services
{
    public class CodeRunQueryService(
        IMapper mapper,
        ICodeRunRepository codeRunRepository,
        ILogger<CodeRunQueryService> logger)
        : ICodeRunQueryService
    {
        public async Task<ReadCodeRunListResponse> GetAllAsync(PaginationQuery? paginationQuery)
        {
            logger.LogInformation($"{nameof(CodeRunQueryService)} {nameof(GetAllAsync)}");

            List<CodeRun> codeRuns;
            if (paginationQuery != null)
            {
                codeRuns = await codeRunRepository.GetAllAsync(paginationQuery);

                return new ReadCodeRunListResponse
                {
                    Items = new ItemWrapper<CodeRunListResponseItem>
                    {
                        Values = mapper.Map<List<CodeRunListResponseItem>>(codeRuns)
                    },
                    Pagination = new PaginationResponse
                    {
                        PageNumber = paginationQuery.PageNumber,
                        PageSize = paginationQuery.PageSize,
                        ItemsCount = codeRuns.Count
                    }
                };
            }

            codeRuns = await codeRunRepository.GetAllAsync();

            return new ReadCodeRunListResponse
            {
                Items = new ItemWrapper<CodeRunListResponseItem>
                {
                    Values = mapper.Map<List<CodeRunListResponseItem>>(codeRuns)
                }
            };
        }

        public async Task<ReadCodeRunResponse> GetByIdAsync(Guid entityId)
        {
            var codeRun = await codeRunRepository.GetByIdAsync(entityId);
            if (codeRun == null)
            {
                throw new DbEntityNotFoundException("CodeRun", entityId);
            }

            return mapper.Map<ReadCodeRunResponse>(codeRun);
        }
    }
}