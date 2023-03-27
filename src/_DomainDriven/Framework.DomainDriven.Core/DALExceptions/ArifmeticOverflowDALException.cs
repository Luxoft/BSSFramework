using Framework.Validation;

namespace Framework.DomainDriven;

public class ArifmeticOverflowDALException : DALException<string>
{
    public ArifmeticOverflowDALException(string args, string message) : base(args, message)
    {
    }

    public override ValidationException Convert()
    {
        return new ValidationException(this.Message);
    }
}
