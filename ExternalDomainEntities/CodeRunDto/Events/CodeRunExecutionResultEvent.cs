namespace ExternalDomainEntities.CodeRunDto.Events;

public class CodeRunExecutionResultEvent
{
    public Guid CodeRunId { get; init; }
    public bool IsSuccess { get; init; }
}