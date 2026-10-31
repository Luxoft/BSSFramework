using System.Net.Mail;

using Anch.Core;

using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.MailMessageModifier;

public class RedirectToSupportMailMessageModifier(IOptions<SmtpSettings> settings) : IMailMessageModifier
{
    /// <summary>
    /// Fallback for messages without any recipient (To/CC/Bcc all empty): sends them to <c>DefaultReceiverEmails</c> instead of losing them.
    /// Order matters: it must run after modifiers that add or remove recipients, otherwise the "empty" check sees an outdated list.
    /// </summary>
    public void Modify(MailMessage message)
    {
        if (message.To.Count == 0 && message.CC.Count == 0 && message.Bcc.Count == 0)
        {
            message.To.AddRange(settings.Value.DefaultReceiverEmails.Select(x => new MailAddress(x)));
        }
    }
}
