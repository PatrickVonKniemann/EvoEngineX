using AutoMapper;
using DomainEntities.CodebaseDto.Query;
using DomainEntities.CodeBaseDto.Query;
using ExternalDomainEntities.CodeBaseDto.Command;

namespace DomainEntities;

public class CodebaseProfile : Profile
{
    public CodebaseProfile()
    {
        CreateMap<CodeBase, ReadCodebaseResponse>().ReverseMap();
        
        // Create
        CreateMap<CreateCodeBaseResponse, ReadCodebaseResponse>().ReverseMap();
        CreateMap<CodeBase, CreateCodeBaseResponse>().ReverseMap();
        CreateMap<CreateCodeBaseRequest, CreateCodeBaseResponse>().ReverseMap();
        CreateMap<CodeBase, CreateCodeBaseRequest>().ReverseMap();

        // Read
        CreateMap<CodeBase, CodebaseListResponseItem>().ReverseMap();

        // Update
        CreateMap<CodeBase, UpdateCodeBaseRequest>().ReverseMap();
        CreateMap<CodeBase, UpdateCodeBaseResponse>().ReverseMap();
        

    }
}