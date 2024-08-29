namespace ExternalDomainEntities;

public class CodeExecutionNotificationEvent
{
    public Guid CodeRunId { get; set; }  // Unique identifier for the code run
    public bool IsSuccess { get; set; }    // Indicates if the code passed validation
}