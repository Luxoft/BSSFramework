namespace Framework.Application.DALExceptions;

public class ArifmeticOverflowDALException : DALException<string>
{
    public ArifmeticOverflowDALException(string args, string message) : base(args, message)
    {
    }
}
