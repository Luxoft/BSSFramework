namespace Framework.Application.DALExceptions;

public abstract class DalException : Exception
{
    protected DalException(string message)
            : base(message)
    {

    }
}
