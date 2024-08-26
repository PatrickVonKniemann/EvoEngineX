namespace CodeExecutionService;

public interface ICodeExecuteService
{
    Task<bool> ExecuteAsync(string code);
}