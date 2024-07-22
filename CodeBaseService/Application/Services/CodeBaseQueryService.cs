using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using Common.Exceptions;
using DomainEntities;
using ExternalDomainEntities.CodeBaseDto.Query;
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
                    Items = new ItemWrapper<ReadCodeBaseListResponseItem>
                    {
                        Values = mapper.Map<List<ReadCodeBaseListResponseItem>>(codebases)
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
                Items = new ItemWrapper<ReadCodeBaseListResponseItem>
                {
                    Values = mapper.Map<List<ReadCodeBaseListResponseItem>>(codebases)
                }
            };
        }

        public async Task<ReadCodeBaseListByUserIdResponse> GetAllByUserIdAsync(Guid userId)
        {
            logger.LogInformation($"{nameof(CodeBaseQueryService)} {nameof(GetAllByUserIdAsync)}");
            var codebases = await codeBaseRepository.GetAllByUserIdAsync(userId);
            return new ReadCodeBaseListByUserIdResponse
            {
                CodeBaseListResponseItems = mapper.Map<List<ReadCodeBaseListResponseItem>>(codebases)
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