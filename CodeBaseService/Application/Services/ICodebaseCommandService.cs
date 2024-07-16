using DomainEntities.CodeBaseDto.Command;
using Generics.BaseEntities;

namespace CodebaseService.Application.Services;

public interface ICodebaseCommandService : IGenericCommandService<CreateCodebaseRequest, UpdateCodebaseRequest, CreateCodebaseResponse, UpdateCodebaseResponse>
{
    
}