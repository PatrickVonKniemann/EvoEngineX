namespace CodeFormaterService.Services;

public interface ICodeValidationService
{
    Task<bool> ValidateAsync(string code);
}