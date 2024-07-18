using DomainEntities.CodeBaseDto.Command;
using Generics.BaseEntities;

namespace CodebaseService.Application.Services;

public interface ICodeBaseCommandService : IGenericCommandService<CreateCodeBaseRequest, UpdateCodeBaseRequest, CreateCodeBaseResponse, UpdateCodeBaseResponse>
{
    
}