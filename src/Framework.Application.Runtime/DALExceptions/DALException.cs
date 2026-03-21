namespace Framework.Application.DALExceptions;

public abstract class DALException : Exception
{
    protected DALException(string message)
            : base(message)
    {

    }
}
