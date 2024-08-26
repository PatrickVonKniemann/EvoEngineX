namespace ExternalDomainEntities.CodeRunDto.Events;

public class CodeRunExecutionResultEvent
{
    public Guid CodeRunId { get; set; }
    public bool IsSuccess { get; set; }
}