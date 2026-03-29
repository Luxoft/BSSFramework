namespace Framework.Validation;

/// <summary>
/// Legacy
/// </summary>
public class ValidationException : ValidationExceptionBase
{
    public ValidationException(string format, params object[] args)
            : this(string.Format(format, args))
    {
    }

    public ValidationException(string message)
            : base(message)
    {
    }
    public ValidationException(string message, Exception inner)
            : base(message, inner)
    {
    }
}
