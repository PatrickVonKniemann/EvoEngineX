namespace ExternalDomainEntities
{
    public class CodeFormaterValidationResultEvent
    {
        public Guid CodeRunId { get; set; }  // Unique identifier for the code run
        public bool IsValid { get; set; }    // Indicates if the code passed validation
        public string? ErrorMessage { get; set; } // Optional: Error message if validation failed
    }
}