using DomainEntities.UserDto.Query;
using Generics.BaseEntities;

namespace UserService.Application.Services;

public interface IUserQueryService : IGenericEntityQueryService<ReadUserResponse, ReadUserListResponse>
{
    
}