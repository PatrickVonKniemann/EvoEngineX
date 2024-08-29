namespace CodeExecutionService;

public interface ICodeExecutionLogic
{
    public Task<bool> ExecuteAsync(string code);
}