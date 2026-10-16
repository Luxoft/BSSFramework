using System.Net.Mail;

using Framework.Core;

namespace Framework.Notification;

public class SmtpNotificationMessageSender(
    IMessageSender<MailMessage> mailMessageSender) : IMessageSender<Notification.Domain.Notification>
{
    public Task SendAsync(Domain.Notification notification, CancellationToken ct) => mailMessageSender.SendAsync(notification.Message, ct);
}
