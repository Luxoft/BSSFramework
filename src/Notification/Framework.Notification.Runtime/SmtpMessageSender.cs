using System.Net.Mail;

using Framework.Core;

using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class SmtpMessageSender(
    ISmtpClientFactory smtpClientFactory,
    ILogger<SmtpMessageSender> logger) : IMessageSender<MailMessage>
{
    public async Task SendAsync(MailMessage message, CancellationToken ct)
    {
        try
        {
            using var client = smtpClientFactory.CreateSmtpClient();

            await client.SendMailAsync(message, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send notification to smtp server");

            throw;
        }
    }
}
