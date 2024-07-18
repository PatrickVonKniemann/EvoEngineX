using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using Common.Exceptions;
using DomainEntities.CodebaseDto.Query;
using DomainEntities.CodeBaseDto.Query;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodeBaseService.Application.Services
{
    public class CodeBaseQueryService(IMapper mapper, ICodeBaseRepository codeBaseRepository, ILogger<CodeBaseQueryService> logger)
        : ICodeBaseQueryService
    {
        public async Task<ReadCodebaseListResponse> GetAllAsync(PaginationQuery paginationQuery)
        {
            logger.LogInformation($"{nameof(CodeBaseQueryService)} {nameof(GetAllAsync)}");

            var codebases = await codeBaseRepository.GetAllAsync(paginationQuery);

            return new ReadCodebaseListResponse
            {
                Items = new ItemWrapper<CodebaseListResponseItem>
                {
                    Values = mapper.Map<List<CodebaseListResponseItem>>(codebases)
                },
                Pagination = new PaginationResponse
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                    ItemsCount = codebases.Count
                }
            };
        }

        public async Task<ReadCodebaseResponse> GetByIdAsync(Guid entityId)
        {
            var user = await codeBaseRepository.GetByIdAsync(entityId);
            if (user == null)
            {
                throw new DbEntityNotFoundException("CodeBase", entityId);
            }
            return mapper.Map<ReadCodebaseResponse>(user);
        }
    }
}
