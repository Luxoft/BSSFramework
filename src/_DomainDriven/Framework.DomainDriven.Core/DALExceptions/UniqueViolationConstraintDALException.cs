using Framework.Core;
using Framework.Validation;

namespace Framework.DomainDriven;

public class UniqueViolationConstraintDALException : DALException<UniqueConstraint>
{
    public UniqueViolationConstraintDALException(UniqueConstraint args)
            : base(args, GetMessage(args))
    {

    }

    public override ValidationException Convert()
    {
        return new ValidationException(GetMessage(this.Args));
    }

    private static string GetMessage(UniqueConstraint constraint)
    {
        return $"{constraint.ObjectInfo.Type.GetValidationName()} with same:'{constraint.Properties.Join(",")}' already exists";
    }
}
