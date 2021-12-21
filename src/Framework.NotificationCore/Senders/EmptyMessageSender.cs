using Framework.Core;
using Framework.Notification.DTO;

namespace Framework.NotificationCore.Senders
{
    public class EmptyMessageSender : IMessageSender<NotificationEventDTO>
    {
        public void Send(NotificationEventDTO message, TransactionMessageMode sendMessageMode = TransactionMessageMode.Auto)
        {
        }
    }
}
