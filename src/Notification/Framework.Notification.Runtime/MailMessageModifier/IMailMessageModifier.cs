using System.Net.Mail;

namespace Framework.Notification.MailMessageModifier;

public interface IMailMessageModifier
{
    public const string LoggerKey = "Logger";

    void Modify(MailMessage message);
}
