namespace Framework.Application;

public class BusinessLogicException : Exception
{
    public BusinessLogicException(string message, Exception exception)
        : base(message, exception)
    {
    }

    public BusinessLogicException(string message)
        : base(message)
    {
    }

    public BusinessLogicException(string message, params object[] args)
        : this(string.Format(message, args))
    {
    }
}
