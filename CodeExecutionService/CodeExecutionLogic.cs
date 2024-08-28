namespace CodeExecutionService;

public class CodeExecutionLogic : ICodeExecutionLogic
{
    public Task<bool> ExecuteAsync(string code)
    {
        Thread.Sleep(1000);
        return FlipCoin();
    }
    
    private Task<bool> FlipCoin()
    {
        var random = new Random();
        return Task.FromResult(random.Next(0, 2) == 0);
    }
}