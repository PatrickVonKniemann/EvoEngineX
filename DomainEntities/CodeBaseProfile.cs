using AutoMapper;
using DomainEntities.CodebaseDto.Query;
using DomainEntities.CodeBaseDto.Query;
using ExternalDomainEntities.CodeBaseDto.Command;

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
        CreateMap<CodeBase, CodeBaseListResponseItem>().ReverseMap();

        // Update
        CreateMap<CodeBase, UpdateCodeBaseRequest>().ReverseMap();
        CreateMap<CodeBase, UpdateCodeBaseResponse>().ReverseMap();
        

    }
}