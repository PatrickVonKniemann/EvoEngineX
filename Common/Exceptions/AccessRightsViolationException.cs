namespace Common.Exceptions;

/// <summary>
/// Base exception on DB context
/// </summary>
public class AccessRightsViolationException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public AccessRightsViolationException(string message) : base(message)
    {
    }
}