using AutoMapper;
using Generics.BaseEntities;
using CodeRunService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Command;
using Generics.Enums;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;

namespace CodeRunService.Application.Services;

public class CodeRunCommandService(
    ILogger<CodeRunCommandService> logger,
    IMapper mapper,
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher)
    : BaseCommandService<CodeRun, CreateCodeRunDetailRequest, CreateCodeRunResponse, UpdateCodeRunRequest,
        UpdateCodeRunResponse>(mapper, codeRunRepository, logger), ICodeRunCommandService
{
    public async Task<CreateCodeRunResponse> HandleAddAsync(CreateCodeRunRequest req)
    {
        var newCodeRun = CreateNewCodeRunRequest(req);
        var createResponse = await AddAsync(newCodeRun);

        await PublishCodeValidationRequestedEventAsync(createResponse.Id, newCodeRun.Code);

        return new CreateCodeRunResponse
        {
            Id = createResponse.Id,
            Status = "Accepted",
            Message = "Code run creation has started and will be processed."
        };
    }


    private async Task PublishCodeValidationRequestedEventAsync(Guid codeRunId, string code)
    {
        await eventPublisher.PublishAsync(new CodeRunValidationRequestedEvent
        {
            CodeRunId = codeRunId,
            Code = code
        }, EventQueueList.CodeValidationQueue);
    }

    private CreateCodeRunDetailRequest CreateNewCodeRunRequest(CreateCodeRunRequest req)
    {
        return new CreateCodeRunDetailRequest
        {
            CodeBaseId = req.CodeBaseId,
            Code = req.Code,
            Status = RunStatus.Created,
            RunStart = DateTimeOffset.UtcNow.UtcDateTime
        };
    }
}