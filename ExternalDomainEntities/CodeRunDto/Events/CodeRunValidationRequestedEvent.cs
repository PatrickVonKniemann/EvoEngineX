namespace ExternalDomainEntities.CodeRunDto.Events;

public class CodeRunValidationRequestedEvent
{
    public Guid CodeRunId { get; set; }
    public string Code { get; set; }
}