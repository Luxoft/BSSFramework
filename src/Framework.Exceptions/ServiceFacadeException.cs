namespace Framework.Exceptions;

// TODO: »справить
public class ServiceFacadeException : Exception
{
    public ServiceFacadeException(Exception innerException, string format, params object[] args)
            : this(innerException, string.Format(format, args))
    {

    }
    public ServiceFacadeException(Exception innerException, string message)
            : base(message, innerException)
    {

    }

    public ServiceFacadeException(string format, params object[] args)
            : this(null, string.Format(format, args))
    {

    }

    public ServiceFacadeException(string message)
            : this(null, message)
    {

    }

    protected ServiceFacadeException ()
    {
    }
}
