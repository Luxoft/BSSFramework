namespace Framework.Exceptions;

public class IntergationException : ServiceFacadeException
{
    public IntergationException(Exception innerException, string format, params object[] args) : base(innerException, format, args)
    {
    }

    public IntergationException(Exception innerException, string message) : base(innerException, message)
    {
    }

    public IntergationException(string format, params object[] args) : base(format, args)
    {
    }

    public IntergationException(string message) : base(message)
    {
    }
}
