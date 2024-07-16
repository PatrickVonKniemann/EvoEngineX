using AutoMapper;
using DomainEntities.CodeBaseDto.Command;
using DomainEntities.CodebaseDto.Query;
using DomainEntities.CodeBaseDto.Query;

namespace DomainEntities.CodebaseDto;

public class CodebaseProfile : Profile
{
    public CodebaseProfile()
    {
        CreateMap<Codebase, ReadCodebaseResponse>().ReverseMap();
        
        // Create
        CreateMap<CreateCodebaseResponse, ReadCodebaseResponse>().ReverseMap();
        CreateMap<Codebase, CreateCodebaseResponse>().ReverseMap();
        CreateMap<CreateCodebaseRequest, CreateCodebaseResponse>().ReverseMap();
        CreateMap<Codebase, CreateCodebaseRequest>().ReverseMap();

        // Read
        CreateMap<Codebase, CodebaseListResponseItem>().ReverseMap();

        // Update
        CreateMap<Codebase, UpdateCodebaseRequest>().ReverseMap();
        CreateMap<Codebase, UpdateCodebaseResponse>().ReverseMap();
        

    }
}