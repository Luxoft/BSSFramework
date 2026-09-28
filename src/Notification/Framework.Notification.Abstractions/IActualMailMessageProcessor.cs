using System.Net.Mail;

namespace Framework.Notification;

public interface IActualMailMessageProcessor
{
    MailMessage GetActualMailMessage(MailMessage baseMessage);
}
