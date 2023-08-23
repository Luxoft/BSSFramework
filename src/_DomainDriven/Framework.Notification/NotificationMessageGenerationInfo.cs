using Framework.Core;
using Framework.Configuration;

namespace Framework.Notification;

public class NotificationMessageGenerationInfo
{
    public NotificationMessageGenerationInfo(string emails, object currentRoot, object previousRoot)
        : this(DefaultEmployee.CreateMany(emails), currentRoot, previousRoot)
    {

    }

    public NotificationMessageGenerationInfo(IEmployee recipient, object currentRoot, object previousRoot)
        : this(recipient.MaybeYield(), currentRoot, previousRoot)
    {

    }

    public NotificationMessageGenerationInfo(IEnumerable<IEmployee> recipients, object currentRoot, object previousRoot)
    {
        this.Recipients = recipients.EmptyIfNull().ToReadOnlyCollection();
        this.CurrentRoot = currentRoot;
        this.PreviousRoot = previousRoot;
    }


    public IEnumerable<IEmployee> Recipients { get; private set; }

    public object CurrentRoot { get; private set; }

    public object PreviousRoot { get; private set; }


    private class DefaultEmployee : IEmployee
    {
        private DefaultEmployee(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

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
