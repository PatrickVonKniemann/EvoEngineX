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
    public class CodebaseQueryService(IMapper mapper, ICodebaseRepository codebaseRepository, ILogger<CodebaseQueryService> logger)
        : ICodebaseQueryService
    {
        public async Task<ReadCodebaseListResponse> GetAllAsync(PaginationQuery paginationQuery)
        {
            logger.LogInformation($"{nameof(CodebaseQueryService)} {nameof(GetAllAsync)}");

            var codebases = await codebaseRepository.GetAllAsync(paginationQuery);

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
            var user = await codebaseRepository.GetByIdAsync(entityId);
            if (user == null)
            {
                throw new DbEntityNotFoundException("Codebase", entityId);
            }
            return mapper.Map<ReadCodebaseResponse>(user);
        }
    }
}
