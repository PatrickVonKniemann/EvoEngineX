using AutoMapper;
using DomainEntities;
using ExternalDomainEntities.UserDto.Query;
using Generics.BaseEntities;
using Generics.Exceptions;
using Generics.Pagination;
using UserService.Infrastructure.Database;

namespace UserService.Application.Services
{
    public class UserQueryService(IMapper mapper, IUserRepository userRepository, ILogger<UserQueryService> logger)
        : IUserQueryService
    {
        public async Task<ReadUserListResponse> GetAllAsync(PaginationQuery? paginationQuery)
        {
            logger.LogInformation($"{nameof(UserQueryService)} {nameof(GetAllAsync)}");

            List<User> users;
            if (paginationQuery != null)
            {
                users = await userRepository.GetAllAsync(paginationQuery);
                var itemsCount = await userRepository.GetCount();
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
                        ItemsCount = itemsCount,
                        TotalPages = (int)Math.Ceiling((double)itemsCount / paginationQuery.PageSize)
                    }
                };
            }
            else
            {
                users = await userRepository.GetAllAsync();
                return new ReadUserListResponse
                {
                    Items = new ItemWrapper<UserListResponseItem>
                    {
                        Values = mapper.Map<List<UserListResponseItem>>(users)
                    }
                };
            }
        }

        public async Task<ReadUserResponse> GetByIdAsync(Guid entityId)
        {
            var user = await userRepository.GetByIdAsync(entityId);
            if (user == null)
            {
                throw new DbEntityNotFoundException("User", entityId);
            }

            return mapper.Map<ReadUserResponse>(user);
        }
    }
}