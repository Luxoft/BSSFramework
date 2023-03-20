using System.Net.Mail;

namespace Framework.Notification;

public interface IMailAddressContainer
{
    MailAddress Sender { get; }
}
