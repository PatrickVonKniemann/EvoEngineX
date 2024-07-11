using AutoMapper;
using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using Generics.BaseEntities;
using Generics.Pagination;
using UsersService.Database;


namespace UsersService.Services
{
    public class UserQueryService(IMapper mapper, IUserRepository userRepository, ILogger<UserQueryService> logger)
        : IUserQueryService
    {
        public async Task<ReadUserListResponse> GetAllAsync(PaginationQuery paginationQuery)
        {
            logger.LogInformation($"{nameof(UserQueryService)} {nameof(GetAllAsync)}");

            var users = await userRepository.GetAllAsync(paginationQuery);

            return new ReadUserListResponse
            {
                Items = new ItemWrapper<UserListResponseItem>
                {
                    Values = mapper.Map<List<UserListResponseItem>>(users)
                },
                Pagination = new PaginationResponse
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                    ItemsCount = users.Count
                }
            };
        }

        public async Task<ReadUserResponse> GetByIdAsync(Guid entityId)
        {
            var user = await userRepository.GetByIdAsync(entityId);
            return mapper.Map<ReadUserResponse>(user);
        }
    }
}
