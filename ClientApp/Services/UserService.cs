using ExternalDomainEntities.UserDto.Command;
using ExternalDomainEntities.UserDto.Query;

namespace ClientApp.Services;

public class UserService(HttpClient httpClient, ILogger<UserService> logger)
    : GenericService<ReadUserListResponse, ReadUserResponse, ReadUserListRequest, CreateUserRequest, UpdateUserRequest>(
        httpClient, $"{ServiceUrls.UserServiceUrl}/user", logger);