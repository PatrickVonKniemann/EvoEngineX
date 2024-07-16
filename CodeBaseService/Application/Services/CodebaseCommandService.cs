using AutoMapper;
using CodebaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using DomainEntities;
using DomainEntities.CodeBaseDto.Command;

namespace CodeBaseService.Application.Services;

public class CodebaseCommandService(
    ILogger<CodebaseCommandService> logger,
    IMapper mapper,
    ICodebaseRepository codebaseRepository)
    : ICodebaseCommandService
{
    public async Task<CreateCodebaseResponse> AddAsync(CreateCodebaseRequest entityRequest)
    {
        logger.LogInformation($"{nameof(CodebaseCommandService)} {nameof(AddAsync)}");
        var user = await codebaseRepository.AddAsync(mapper.Map<Codebase>(entityRequest));
        return mapper.Map<CreateCodebaseResponse>(user);
    }

    public async Task<UpdateCodebaseResponse> UpdateAsync(Guid entityId, UpdateCodebaseRequest entityRequest)
    {
        logger.LogInformation($"{nameof(CodebaseCommandService)} {nameof(UpdateAsync)}");
        var updatedCodebase = await codebaseRepository.UpdateAsync(entityId, mapper.Map<Codebase>(entityRequest));
        return mapper.Map<UpdateCodebaseResponse>(updatedCodebase);
    }

    public async Task DeleteAsync(Guid entityId)
    {
        logger.LogInformation($"{nameof(CodebaseCommandService)} {nameof(DeleteAsync)}");
        await codebaseRepository.DeleteAsync(entityId);
    }
}