using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Framework.NotificationCore.Extensions;

internal static class RecipientsHelper
{
    public static IEnumerable<MailAddress> ToRecipients(string[] targets) =>
            targets.Select(z => z.Trim()).Distinct().Select(z => new MailAddress(z));
}
