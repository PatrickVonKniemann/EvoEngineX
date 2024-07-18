using AutoMapper;
using DomainEntities;
using CodeRunService.Infrastructure.Database;
using ExternalDomainEntities.CodeRunDto.Command;

namespace CodeRunService.Application.Services;

public class CodeRunCommandService(
    ILogger<CodeRunCommandService> logger,
    IMapper mapper,
    ICodeRunRepository codeRunRepository)
    : ICodeRunCommandService
{
    public async Task<CreateCodeRunResponse> AddAsync(CreateCodeRunRequest entityRequest)
    {
        logger.LogInformation($"{nameof(CodeRunCommandService)} {nameof(AddAsync)}");
        var user = await codeRunRepository.AddAsync(mapper.Map<CodeRun>(entityRequest));
        return mapper.Map<CreateCodeRunResponse>(user);
    }

    public async Task<UpdateCodeRunResponse> UpdateAsync(Guid entityId, UpdateCodeRunRequest entityRequest)
    {
        logger.LogInformation($"{nameof(CodeRunCommandService)} {nameof(UpdateAsync)}");
        var updatedCodeRun = await codeRunRepository.UpdateAsync(entityId, mapper.Map<CodeRun>(entityRequest));
        return mapper.Map<UpdateCodeRunResponse>(updatedCodeRun);
    }

    public async Task DeleteAsync(Guid entityId)
    {
        logger.LogInformation($"{nameof(CodeRunCommandService)} {nameof(DeleteAsync)}");
        await codeRunRepository.DeleteAsync(entityId);
    }
}