using Framework.Core.MessageSender;
using Framework.Notification.DTO;

namespace Framework.Configuration.BLL.Notification;

public class NotificationMessageSender(IMessageSender<NotificationEventDTO> notificationEventSender, IDefaultMailSenderContainer defaultMailSenderContainer)
    : IMessageSender<Framework.Notification.Domain.Notification>
{
    public async Task SendAsync(Framework.Notification.Domain.Notification notification, CancellationToken cancellationToken)
    {
        notification.Message.Sender = notification.Message.Sender ?? defaultMailSenderContainer.DefaultSender;

        await notificationEventSender.SendAsync(new NotificationEventDTO(notification), cancellationToken);
    }
}
