using System.Collections.Immutable;

using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public record NotificationMessageGenerationInfo<TRenderingObject>(ImmutableArray<IEmployee> Recipients, DomainObjectVersions<TRenderingObject> Versions)
    where TRenderingObject : class
{
    public NotificationMessageGenerationInfo(string emails, DomainObjectVersions<TRenderingObject> version)
        : this([.. DefaultEmployee.CreateMany(emails)], version)
    {
    }

    public NotificationMessageGenerationInfo(IEmployee? recipient, DomainObjectVersions<TRenderingObject> version)
        : this([.. recipient.MaybeYield()], version)
    {
    }

    private class DefaultEmployee(string email) : IEmployee
    {
        public string Email { get; } = email;

        public string Login => throw new NotImplementedException();

        public static IEnumerable<DefaultEmployee> CreateMany(string emails) =>

            from email in emails.TrimNull().Split([',', ';'], StringSplitOptions.RemoveEmptyEntries)

            select new DefaultEmployee(email.Trim());
    }
}
