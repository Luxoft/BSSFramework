using System.Net.Mail;

using Anch.Core;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Notification.MailMessageModifier;

using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class SmtpMessageSender(
    ISmtpClientFactory smtpClientFactory,
    IEnumerable<IMailMessageModifier> mailMessageModifiers,
    ILogger<SmtpMessageSender> logger) : IMessageSender<MailMessage>
{
    public async Task SendAsync(MailMessage baseMessage, CancellationToken cancellationToken)
    {
        try
        {
            var actualMailMessage = this.GetActualMailMessage(baseMessage);

            using var client = smtpClientFactory.CreateSmtpClient();

            await client.SendMailAsync(actualMailMessage, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send notification to smtp server");

            throw;
        }
    }

    protected virtual MailMessage GetActualMailMessage(MailMessage baseMessage)
    {
        var newMailMessage = baseMessage.Clone();

        mailMessageModifiers.Foreach(m => m.Modify(newMailMessage));

        return newMailMessage;
    }
}
