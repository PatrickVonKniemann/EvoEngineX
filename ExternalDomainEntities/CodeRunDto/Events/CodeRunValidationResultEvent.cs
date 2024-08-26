namespace ExternalDomainEntities.CodeRunDto.Events;

public class CodeRunValidationResultEvent
{
    public Guid CodeRunId { get; set; }
    public bool IsValid { get; set; }
}
