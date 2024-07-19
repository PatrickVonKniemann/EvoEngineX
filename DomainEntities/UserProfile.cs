using AutoMapper;
using DomainEntities.UserDto.Command;
using DomainEntities.UserDto.Query;
using ExternalDomainEntities.UserDto.Command;

namespace DomainEntities;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ReadUserResponse>().ReverseMap();
        
        // Create
        CreateMap<CreateUserResponse, ReadUserResponse>().ReverseMap();
        CreateMap<User, CreateUserResponse>().ReverseMap();
        CreateMap<CreateUserRequest, CreateUserResponse>().ReverseMap();
        CreateMap<User, CreateUserRequest>().ReverseMap();

        // Read
        CreateMap<User, UserListResponseItem>().ReverseMap();

        // Update
        CreateMap<User, UpdateUserRequest>().ReverseMap();
        CreateMap<User, UpdateUserResponse>().ReverseMap();
        

    }
}