using System.Net.Mail;

using Anch.Core;

using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.MailMessageModifier;

public class RedirectToSupportMailMessageModifier(IOptions<SmtpSettings> settings) : IMailMessageModifier
{
    public void Modify(MailMessage message)
    {
        if (message.To.Count == 0 && settings.Value.DefaultReceiverEmails != null)
        {
            message.To.AddRange(settings.Value.DefaultReceiverEmails.Select(x => new MailAddress(x)));
        }
    }
}
