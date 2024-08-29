namespace ExternalDomainEntities;

public class CodeExecutionRequestEvent
{
    public Guid CodeRunId { get; set; }  // Unique identifier for the code run
    public string Code { get; set; }    // Indicates if the code passed validation
    public string? ErrorMessage { get; set; } // Optional: Error message if validation failed
}