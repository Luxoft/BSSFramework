using System.Net.Mail;

using Framework.Core;
using Framework.Notification.DTO;

namespace Framework.Configuration.BLL.Notification;

public class MailMessageSender(IMessageSender<NotificationEventDTO> notificationEventSender, IDefaultMailSenderContainer defaultMailSenderContainer)
    : IMessageSender<MailMessage>
{
    public async Task SendAsync(MailMessage message, CancellationToken cancellationToken)
    {
        message.Sender ??= defaultMailSenderContainer.DefaultSender;

        await notificationEventSender.SendAsync(new NotificationEventDTO(message), cancellationToken);
    }
}
