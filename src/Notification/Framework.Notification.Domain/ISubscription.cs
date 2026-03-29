using System.Net.Mail;

namespace Framework.Notification.Domain;

public interface ISubscription
{
    bool IncludeAttachments { get; set; }

    bool SendIndividualLetters { get; set; }

    MailAddress Sender { get; }
}
