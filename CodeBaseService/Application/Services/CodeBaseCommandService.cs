using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeBaseDto.Command;
using Generics.BaseEntities;

namespace CodeBaseService.Application.Services;

public class CodeBaseCommandService(
    ILogger<CodeBaseCommandService> logger,
    IMapper mapper,
    ICodeBaseRepository codeRunRepository)
    : BaseCommandService<CodeBase, CreateCodeBaseRequest, CreateCodeBaseResponse, UpdateCodeBaseRequest,
        UpdateCodeBaseResponse>(mapper, codeRunRepository, logger), ICodeBaseCommandService
{
    // Any additional methods specific to CodeBaseCommandService can go here
}