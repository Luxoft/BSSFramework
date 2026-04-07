using System.Net.Mail;

namespace Framework.Notification.MailMessageModifier;

public interface IMailMessageModifier
{
    void Modify(MailMessage message);
}
