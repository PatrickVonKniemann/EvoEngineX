namespace CodeExecutionService;

public interface ICodeExecutionLogic
{
    public Task ExecuteAsync(string code);
}