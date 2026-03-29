using System.Collections.Immutable;

using Framework.Core;

namespace Framework.Notification;

public class NotificationMessageGenerationInfo(ImmutableArray<IEmployee> recipients, object currentRoot, object previousRoot)
{
    public NotificationMessageGenerationInfo(string emails, object currentRoot, object previousRoot)
        : this([.. DefaultEmployee.CreateMany(emails)], currentRoot, previousRoot)
    {
    }

    public NotificationMessageGenerationInfo(IEmployee? recipient, object currentRoot, object previousRoot)
        : this([.. recipient.MaybeYield()], currentRoot, previousRoot)
    {
    }

    public ImmutableArray<IEmployee> Recipients { get; private set; } = [.. recipients ];

    public object CurrentRoot { get; private set; } = currentRoot;

    public object PreviousRoot { get; private set; } = previousRoot;

    private class DefaultEmployee : IEmployee
    {
        private DefaultEmployee(string email)
        {
            this.Email = email;
        }


        public string Email { get; private set; }

        public string Login { get; private set; }

        public static IEnumerable<DefaultEmployee> CreateMany(string emails)
        {
            return from email in emails.TrimNull().Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)

                   select new DefaultEmployee(email.Trim());
        }
    }
}
