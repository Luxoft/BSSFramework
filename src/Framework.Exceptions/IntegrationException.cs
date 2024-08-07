namespace Framework.Exceptions;

public class IntegrationException : ServiceFacadeException
{
    public IntegrationException(Exception innerException, string format, params object[] args) : base(innerException, format, args)
    {
    }

    public IntegrationException(Exception innerException, string message) : base(innerException, message)
    {
    }

    public IntegrationException(string format, params object[] args) : base(format, args)
    {
    }

    public IntegrationException(string message) : base(message)
    {
    }
}
