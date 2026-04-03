using System.Net.Mail;

namespace Framework.Notification.Extensions;

internal static class RecipientsHelper
{
    public static IEnumerable<MailAddress> ToRecipients(string[] targets) =>
            targets.Select(z => z.Trim()).Distinct().Select(z => new MailAddress(z));
}
