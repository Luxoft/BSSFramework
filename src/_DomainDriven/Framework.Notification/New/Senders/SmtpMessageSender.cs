using System.Net.Mail;

using Framework.Core;

namespace Framework.Notification.New;

/// <summary>
/// SMTP sender implementation
/// </summary>
public class SmtpMessageSender(Func<SmtpClient> getSmtpClient) : IMessageSender<Message>
{
    /// <inheritdoc />
    public async Task SendAsync(Message message, CancellationToken cancellationToken)
    {
        var client = getSmtpClient();

        var mailMessage = message.ToMailMessage();

        await client.SendMailAsync(mailMessage, cancellationToken);
    }
}
