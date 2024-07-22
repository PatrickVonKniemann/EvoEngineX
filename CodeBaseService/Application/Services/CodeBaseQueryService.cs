using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using Common.Exceptions;
using DomainEntities;
using DomainEntities.CodebaseDto.Query;
using DomainEntities.CodeBaseDto.Query;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodeBaseService.Application.Services
{
    public class CodeBaseQueryService(
        IMapper mapper,
        ICodeBaseRepository codeBaseRepository,
        ILogger<CodeBaseQueryService> logger)
        : ICodeBaseQueryService
    {
        public async Task<ReadCodeBaseListResponse> GetAllAsync(PaginationQuery? paginationQuery)
        {
            logger.LogInformation($"{nameof(CodeBaseQueryService)} {nameof(GetAllAsync)}");


            List<CodeBase> codebases;
            if (paginationQuery != null)
            {
                codebases = await codeBaseRepository.GetAllAsync(paginationQuery);

                return new ReadCodeBaseListResponse
                {
                    Items = new ItemWrapper<CodeBaseListResponseItem>
                    {
                        Values = mapper.Map<List<CodeBaseListResponseItem>>(codebases)
                    },
                    Pagination = new PaginationResponse
                    {
                        PageNumber = paginationQuery.PageNumber,
                        PageSize = paginationQuery.PageSize,
                        ItemsCount = codebases.Count
                    }
                };
            }

            codebases = await codeBaseRepository.GetAllAsync();

            return new ReadCodeBaseListResponse
            {
                Items = new ItemWrapper<CodeBaseListResponseItem>
                {
                    Values = mapper.Map<List<CodeBaseListResponseItem>>(codebases)
                }
            };
        }

        public async Task<ReadCodeBaseResponse> GetByIdAsync(Guid entityId)
        {
            var user = await codeBaseRepository.GetByIdAsync(entityId);
            if (user == null)
            {
                throw new DbEntityNotFoundException("CodeBase", entityId);
            }

            return mapper.Map<ReadCodeBaseResponse>(user);
        }
    }
}