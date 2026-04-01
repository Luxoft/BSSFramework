namespace Framework.Database.DALExceptions;

public abstract class DALException<T>(T args, string message) : DALException(message)
{
    public T Args => args;
}

public class DALException : Exception
{
    public DALException(string message)
        : base(message)
    {
    }

    public DALException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
