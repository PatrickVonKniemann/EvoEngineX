using ExternalDomainEntities.UserDto.Command;
using ExternalDomainEntities.UserDto.Query;

namespace ClientApp.Services;

public class UserService(HttpClient httpClient) : GenericService<ReadUserListResponse, ReadUserResponse, ReadUserListRequest, CreateUserRequest, UpdateUserRequest>(httpClient, "http://localhost:5003/user");