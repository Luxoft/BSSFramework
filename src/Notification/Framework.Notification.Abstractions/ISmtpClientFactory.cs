using System.Net.Mail;

namespace Framework.Notification;

public interface ISmtpClientFactory
{
    SmtpClient CreateSmtpClient();
}
