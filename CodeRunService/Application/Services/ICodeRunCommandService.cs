using ExternalDomainEntities.CodeRunDto.Command;
using Generics.BaseEntities;

namespace CodeRunService.Application.Services;

public interface ICodeRunCommandService : IGenericEntityCommandService<CreateCodeRunDetailRequest, CreateCodeRunResponse, UpdateCodeRunRequest, UpdateCodeRunResponse>
{
    Task<CreateCodeRunResponse> HandleAddAsync(CreateCodeRunRequest newCodeRun);
}