namespace CodeFormaterService.Services;

public class CodeValidationService : ICodeValidationService
{

    public Task<bool> ValidateAsync(string code)
    {
        return FlipCoin();
    }
    
    // TODO this is temporary while testing
    private Task<bool> FlipCoin()
    {
        var random = new Random();
        return Task.FromResult(random.Next(0, 2) == 0);
    }
}