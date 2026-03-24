using CommonFramework;

namespace Framework.Application.DALExceptions;

public class UniqueViolationConstraintDalException(UniqueConstraint args, IDalValidationIdentitySource validationIdentitySource)
    : DalException<UniqueConstraint>(args, GetMessage(args, validationIdentitySource))
{
    private static string GetMessage(UniqueConstraint constraint, IDalValidationIdentitySource validationIdentitySource)
    {
        return $"{validationIdentitySource.GetTypeValidationName(constraint.ObjectInfo.Type)} with same:'{constraint.Properties.Join(",")}' already exists";
    }
}
