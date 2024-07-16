using AutoMapper;
using DomainEntities.CodeRunDto.Command;
using DomainEntities.CodeRunDto.Query;

namespace DomainEntities.CodeRunDto;

public class CodeRunProfile : Profile
{
    public CodeRunProfile()
    {
        CreateMap<CodeRun, ReadCodeRunResponse>().ReverseMap();
        
        // Create
        CreateMap<CreateCodeRunResponse, ReadCodeRunResponse>().ReverseMap();
        CreateMap<CodeRun, CreateCodeRunResponse>().ReverseMap();
        CreateMap<CreateCodeRunRequest, CreateCodeRunResponse>().ReverseMap();
        CreateMap<CodeRun, CreateCodeRunRequest>().ReverseMap();

        // Read
        CreateMap<CodeRun, CodeRunListResponseItem>().ReverseMap();

        // Update
        CreateMap<CodeRun, UpdateCodeRunRequest>().ReverseMap();
        CreateMap<CodeRun, UpdateCodeRunResponse>().ReverseMap();
        

    }
}