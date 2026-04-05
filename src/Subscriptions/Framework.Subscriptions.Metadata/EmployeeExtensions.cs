using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public static class EmployeeExtensions
{
    public static IEnumerable<IEmployee> GetMergeResult(this IEnumerable<IEmployee> recipientsByRoles, IEnumerable<IEmployee> recipientsByGeneration, RecipientsSelectorMode mode)
    {
        if (recipientsByRoles == null) throw new ArgumentNullException(nameof(recipientsByRoles));
        if (recipientsByGeneration == null) throw new ArgumentNullException(nameof(recipientsByGeneration));

        var employeeComparer = EmployeeEqualityComparer.EMail;

        switch (mode)
        {
            case RecipientsSelectorMode.Union:
                return recipientsByRoles.Union(recipientsByGeneration, employeeComparer);

            case RecipientsSelectorMode.Intersect:
                return recipientsByRoles.Intersect(recipientsByGeneration, employeeComparer);

            case RecipientsSelectorMode.LeftExceptRight:
                return recipientsByRoles.Except(recipientsByGeneration, employeeComparer);

            case RecipientsSelectorMode.RightExceptLeft:
                return recipientsByGeneration.Except(recipientsByRoles, employeeComparer);

            default:
                throw new ArgumentOutOfRangeException(mode.ToString());
        }
    }
}
