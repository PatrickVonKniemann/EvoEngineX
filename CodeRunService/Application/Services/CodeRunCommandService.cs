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

    public async Task<CreateCodeRunResponse> HandleAddAsync(CreateCodeRunRequest newCodeRun)
    {
        logger.LogInformation("Starting code run creation process.");

        var newCodeRunRequest = CreateNewCodeRunDetailRequest(newCodeRun);
        var createResponse = await AddAsync(newCodeRunRequest);

        logger.LogInformation("Code run created with Id: {CodeRunId}, publishing validation event.", createResponse.Id);
        await PublishCodeValidationRequestedEventAsync(createResponse.Id, newCodeRunRequest.Code);

        return new CreateCodeRunResponse
        {
            Id = createResponse.Id,
            CodeBaseId = createResponse.CodeBaseId,
            Status = "Accepted",
            Message = "Code run creation has started and will be processed."
        };
    }

    private async Task PublishCodeValidationRequestedEventAsync(Guid codeRunId, string code)
    {
        logger.LogInformation("Publishing CodeRunValidationRequestedEvent for CodeRunId: {CodeRunId}", codeRunId);

        var validationEvent = new CodeRunValidationRequestedEvent
        {
            CodeRunId = codeRunId,
            Code = code
        };

        await eventPublisher.PublishAsync(validationEvent, EventQueueList.CodeValidationQueue);
        logger.LogInformation("CodeRunValidationRequestedEvent published successfully for CodeRunId: {CodeRunId}", codeRunId);
    }

    private CreateCodeRunDetailRequest CreateNewCodeRunDetailRequest(CreateCodeRunRequest request)
    {
        logger.LogInformation("Mapping CreateCodeRunRequest to CreateCodeRunDetailRequest.");

        return new CreateCodeRunDetailRequest
        {
            CodeBaseId = request.CodeBaseId,
            Code = request.Code,
            Status = RunStatus.Validating,
            RunStart = DateTimeOffset.UtcNow.UtcDateTime
        };
    }
}
