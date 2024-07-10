namespace Common.Exceptions;

public class InternalServerErrorException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public InternalServerErrorException(string message) : base(message) { }
}