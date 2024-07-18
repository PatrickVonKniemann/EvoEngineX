using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeBaseDto.Command;

namespace CodeBaseService.Application.Services;

public class CodeBaseCommandService(
    ILogger<CodeBaseCommandService> logger,
    IMapper mapper,
    ICodeBaseRepository codeBaseRepository)
    : ICodeBaseCommandService
{
    public async Task<CreateCodeBaseResponse> AddAsync(CreateCodeBaseRequest entityRequest)
    {
        logger.LogInformation($"{nameof(CodeBaseCommandService)} {nameof(AddAsync)}");
        var user = await codeBaseRepository.AddAsync(mapper.Map<CodeBase>(entityRequest));
        return mapper.Map<CreateCodeBaseResponse>(user);
    }

    public async Task<UpdateCodeBaseResponse> UpdateAsync(Guid entityId, UpdateCodeBaseRequest entityRequest)
    {
        logger.LogInformation($"{nameof(CodeBaseCommandService)} {nameof(UpdateAsync)}");
        var updatedCodebase = await codeBaseRepository.UpdateAsync(entityId, mapper.Map<CodeBase>(entityRequest));
        return mapper.Map<UpdateCodeBaseResponse>(updatedCodebase);
    }

    public async Task DeleteAsync(Guid entityId)
    {
        logger.LogInformation($"{nameof(CodeBaseCommandService)} {nameof(DeleteAsync)}");
        await codeBaseRepository.DeleteAsync(entityId);
    }
}