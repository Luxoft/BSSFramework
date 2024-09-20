using Framework.Core;

namespace Framework.DomainDriven.DALExceptions;

public class UniqueViolationConstraintDALException(UniqueConstraint args, IDalValidationIdentitySource validationIdentitySource)
    : DALException<UniqueConstraint>(args, GetMessage(args, validationIdentitySource))
{
    private static string GetMessage(UniqueConstraint constraint, IDalValidationIdentitySource validationIdentitySource)
    {
        return $"{validationIdentitySource.GetTypeValidationName(constraint.ObjectInfo.Type)} with same:'{constraint.Properties.Join(",")}' already exists";
    }
}
