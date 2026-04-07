using System.Net.Mail;

using CommonFramework;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Notification.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.Notification;

public class ProdSmtpMessageSender(
    ISmtpClientFactory smtpClientFactory,
    IOptions<SmtpSettings> settings,
    ILogger<ProdSmtpMessageSender> logger,
    ISentMessageLogger? sentMessageLogger = null) : IMessageSender<MailMessage>
{
    public async Task SendAsync(MailMessage baseMessage, CancellationToken cancellationToken)
    {
        try
        {
            var actualMailMessage = this.GetActualMailMessage(baseMessage);

            using var client = smtpClientFactory.CreateSmtpClient();

            await client.SendMailAsync(actualMailMessage, cancellationToken);

            if (sentMessageLogger != null)
            {
                await sentMessageLogger.LogAsync(actualMailMessage, cancellationToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send notification to smtp server");

            throw;
        }
    }

    protected virtual MailMessage GetActualMailMessage(MailMessage baseMessage) => this.TryRedirectToSupport(baseMessage);

    private MailMessage TryRedirectToSupport(MailMessage baseMessage)
    {
        if (baseMessage.To.Count == 0 && settings.Value.DefaultReceiverEmails != null)
        {
            var newMailMessage = baseMessage.Clone();
            newMailMessage.To.AddRange(settings.Value.DefaultReceiverEmails.Select(x => new MailAddress(x)));
            return newMailMessage;
        }
        else
        {
            return baseMessage;
        }
    }
}
