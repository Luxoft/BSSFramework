namespace Framework.DomainDriven.DALExceptions;

public abstract class DALException : Exception
{
    protected DALException(string message)
            : base(message)
    {

    }
}
