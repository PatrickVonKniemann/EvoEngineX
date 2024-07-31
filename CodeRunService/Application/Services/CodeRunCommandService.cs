using AutoMapper;
using Generics.BaseEntities;
using CodeRunService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Command;

namespace CodeRunService.Application.Services;

public class CodeRunCommandService(
    ILogger<CodeRunCommandService> logger,
    IMapper mapper,
    ICodeRunRepository codeRunRepository)
    : BaseCommandService<CodeRun, CreateCodeRunRequest, CreateCodeRunResponse, UpdateCodeRunRequest,
        UpdateCodeRunResponse>(mapper, codeRunRepository, logger), ICodeRunCommandService
{
    // Any additional methods specific to CodeRunCommandService can go here
}