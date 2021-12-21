using System.Net.Mail;
using Framework.Core;
using Framework.Notification.DTO;

namespace Framework.NotificationCore.Senders
{
    /// <summary>
    /// заглушка для совместимости со старыми способами отправки нотификаций
    /// </summary>
    public class NotificationContext : DomainDriven.ServiceModel.NotificationContext
    {
        public NotificationContext(IMessageSender<NotificationEventDTO> notificationMessageSender)
            : base(notificationMessageSender, new MailAddress("Fake@Fake.com", "Fake"), "Fake@Fake.com")
        {
        }
    }
}
