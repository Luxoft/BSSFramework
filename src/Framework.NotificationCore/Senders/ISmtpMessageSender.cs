using System.Net.Mail;
using Framework.Notification.DTO;

namespace Framework.NotificationCore.Senders
{
    internal interface ISmtpMessageSender
    {
        void Send(SmtpClient client, NotificationEventDTO message);
    }
}
