namespace ExternalDomainEntities.CodeRunDto.Events;

public class CodeRunExecutionRequestedEvent
{
    public Guid CodeRunId { get; set; }
    public string Code { get; set; }
}
