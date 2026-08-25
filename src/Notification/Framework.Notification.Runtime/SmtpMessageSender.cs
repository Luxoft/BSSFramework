using Framework.Core;

using Microsoft.Extensions.Logging;

namespace Framework.Notification;

public class SmtpMessageSender(
    ISmtpClientFactory smtpClientFactory,
    ILogger<SmtpMessageSender> logger) : IMessageSender<Domain.Notification>
{
    public async Task SendAsync(Domain.Notification notification, CancellationToken ct)
    {
        try
        {
            using var client = smtpClientFactory.CreateSmtpClient();

            await client.SendMailAsync(notification.Message, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send notification to smtp server");

            throw;
        }
    }
}
