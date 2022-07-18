using System.Net.Mail;

namespace Framework.Configuration.BLL.Notification;

public interface IDefaultMailSenderContainer
{
    MailAddress DefaultSender { get; }
}
