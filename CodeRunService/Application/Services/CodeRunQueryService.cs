using AutoMapper;
using Generics.BaseEntities;
using Generics.Pagination;
using CodeRunService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Query;
using Generics.Exceptions;

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
                    Items = new ItemWrapper<ReadCodeRunListResponseItem>
                    {
                        Values = mapper.Map<List<ReadCodeRunListResponseItem>>(codeRuns)
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
                Items = new ItemWrapper<ReadCodeRunListResponseItem>
                {
                    Values = mapper.Map<List<ReadCodeRunListResponseItem>>(codeRuns)
                }
            };
        }


        public async Task<ReadCodeRunListByCodeBaseIdResponse> GetAllByCodeBaseIdAsync(Guid codeBaseId)
        {
            logger.LogInformation($"{nameof(CodeRunQueryService)} {nameof(GetAllByCodeBaseIdAsync)}");
            var coderuns = await codeRunRepository.GetAllByCodeBaseIdAsync(codeBaseId);
            return new ReadCodeRunListByCodeBaseIdResponse
            {
                CodeRunListResponseItems = mapper.Map<List<ReadCodeRunListResponseItem>>(coderuns)
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