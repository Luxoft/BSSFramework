using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public static class EmployeeExtensions
{
    public static IEnumerable<string> GetEmailMergeResult(this IEnumerable<string> recipientsByRoles, IEnumerable<string> recipientsByGeneration, RecipientMergeType mode)
    {
        if (recipientsByRoles == null) throw new ArgumentNullException(nameof(recipientsByRoles));
        if (recipientsByGeneration == null) throw new ArgumentNullException(nameof(recipientsByGeneration));

        var emailComparer = StringComparer.CurrentCultureIgnoreCase;

        switch (mode)
        {
            case RecipientMergeType.Union:
                return recipientsByRoles.Union(recipientsByGeneration, emailComparer);

            case RecipientMergeType.Intersect:
                return recipientsByRoles.Intersect(recipientsByGeneration, emailComparer);

            case RecipientMergeType.LeftExceptRight:
                return recipientsByRoles.Except(recipientsByGeneration, emailComparer);

            case RecipientMergeType.RightExceptLeft:
                return recipientsByGeneration.Except(recipientsByRoles, emailComparer);

            default:
                throw new ArgumentOutOfRangeException(mode.ToString());
        }
    }
}
