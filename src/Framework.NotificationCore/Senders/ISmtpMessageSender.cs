using System.Net.Mail;

using Framework.Notification.DTO;

namespace Framework.NotificationCore.Senders;

internal interface ISmtpMessageSender
{
    Task SendAsync(SmtpClient client, NotificationEventDTO message, CancellationToken cancellationToken);
}
