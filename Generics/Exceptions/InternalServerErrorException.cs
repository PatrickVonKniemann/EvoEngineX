namespace Generics.Exceptions;

public abstract class InternalServerErrorException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    protected InternalServerErrorException(string message) : base(message) { }
}