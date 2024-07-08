using AutoMapper;
using DomainEntities.Users.Request;
using DomainEntities.Users.Response;

namespace DomainEntities.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ReadUserResponse>().ReverseMap();
        
        // Create
        CreateMap<CreateUserResponse, ReadUserResponse>().ReverseMap();
        CreateMap<User, CreateUserResponse>().ReverseMap();
        CreateMap<CreateUserRequest, CreateUserResponse>().ReverseMap();
        
        CreateMap<User, UserListResponseItem>().ReverseMap();
        CreateMap<User, UpdateUserRequest>().ReverseMap();
        CreateMap<User, CreateUserRequest>().ReverseMap();
    }
}