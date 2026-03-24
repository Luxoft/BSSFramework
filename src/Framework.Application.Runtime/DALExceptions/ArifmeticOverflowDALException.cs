namespace Framework.Application.DALExceptions;

public class ArifmeticOverflowDalException : DalException<string>
{
    public ArifmeticOverflowDalException(string args, string message) : base(args, message)
    {
    }
}
