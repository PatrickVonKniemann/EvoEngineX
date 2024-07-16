using DomainEntities.CodeRunDto.Command;
using Generics.BaseEntities;

namespace CodeRunService.Application.Services;

public interface ICodeRunCommandService : IGenericCommandService<CreateCodeRunRequest, UpdateCodeRunRequest, CreateCodeRunResponse, UpdateCodeRunResponse>
{
    
}