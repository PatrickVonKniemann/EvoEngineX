using AutoMapper;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;

namespace DomainEntities;

public class CodebaseProfile : Profile
{
    public CodebaseProfile()
    {
        CreateMap<CodeBase, ReadCodeBaseResponse>().ReverseMap();
        
        // Create
        CreateMap<CreateCodeBaseResponse, ReadCodeBaseResponse>().ReverseMap();
        CreateMap<CodeBase, CreateCodeBaseResponse>().ReverseMap();
        CreateMap<CreateCodeBaseRequest, CreateCodeBaseResponse>().ReverseMap();
        CreateMap<CodeBase, CreateCodeBaseRequest>().ReverseMap();

        // Read
        CreateMap<CodeBase, ReadCodeBaseListResponseItem>().ReverseMap();
        CreateMap<CodeBase, ReadCodeBaseListByUserIdResponse>().ReverseMap();

        // Update
        CreateMap<CodeBase, UpdateCodeBaseRequest>().ReverseMap();
        CreateMap<CodeBase, UpdateCodeBaseResponse>().ReverseMap();
        

    }
}