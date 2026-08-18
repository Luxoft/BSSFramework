using System.Net.Mail;

using Framework.Core;

using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class SmtpMessageSender(
    ISmtpClientFactory smtpClientFactory,
    IActualMailMessageProcessor actualMailMessageProcessor,
    ILogger<SmtpMessageSender> logger) : IMessageSender<MailMessage>
{
    public async Task SendAsync(MailMessage baseMessage, CancellationToken ct)
    {
        try
        {
            var actualMailMessage = actualMailMessageProcessor.GetActualMailMessage(baseMessage);

            using var client = smtpClientFactory.CreateSmtpClient();

            await client.SendMailAsync(actualMailMessage, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send notification to smtp server");

            throw;
        }
    }
}
