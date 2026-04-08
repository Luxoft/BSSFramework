using System.Collections.Immutable;

using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public record NotificationMessageGenerationInfo<TRenderingObject>(ImmutableHashSet<string> Recipients, DomainObjectVersions<TRenderingObject> Versions)
    : NotificationMessageGenerationInfo<TRenderingObject, string>(Recipients, Versions)
    where TRenderingObject : class
{
    public NotificationMessageGenerationInfo(string emails, DomainObjectVersions<TRenderingObject> version)
        : this([.. CreateMany(emails).Distinct(StringComparer.CurrentCultureIgnoreCase)], version)
    {
    }

    private static IEnumerable<string> CreateMany(string emails) =>

        from email in emails.TrimNull().Split([',', ';'], StringSplitOptions.RemoveEmptyEntries)

        select email.Trim();
}

public record NotificationMessageGenerationInfo<TRenderingObject, TRecipient>(ImmutableHashSet<TRecipient> Recipients, DomainObjectVersions<TRenderingObject> Versions)
    where TRenderingObject : class;
