using System.Collections.Immutable;

using Framework.Core;
using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public class NotificationMessageGenerationInfo(ImmutableArray<IEmployee> recipients, object? currentRoot, object? previousRoot)
{
    public NotificationMessageGenerationInfo(string emails, object? currentRoot, object? previousRoot)
        : this([.. DefaultEmployee.CreateMany(emails)], currentRoot, previousRoot)
    {
    }

    public NotificationMessageGenerationInfo(IEmployee? recipient, object? currentRoot, object? previousRoot)
        : this([.. recipient.MaybeYield()], currentRoot, previousRoot)
    {
    }

    public ImmutableArray<IEmployee> Recipients { get; private set; } = [.. recipients ];

    public object? CurrentRoot { get; private set; } = currentRoot;

    public object? PreviousRoot { get; private set; } = previousRoot;

    private class DefaultEmployee(string email) : IEmployee
    {
        public string Email { get; } = email;

        public string Login => throw new NotImplementedException();

        public static IEnumerable<DefaultEmployee> CreateMany(string emails) =>
            from email in emails.TrimNull().Split([',', ';'], StringSplitOptions.RemoveEmptyEntries)

            select new DefaultEmployee(email.Trim());
    }
}
