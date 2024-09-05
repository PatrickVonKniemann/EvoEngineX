namespace CodeExecutionService;

public interface ICodeExecutionLogic
{
    public Task ExecuteAsync(Guid codeRunId, string code);
}