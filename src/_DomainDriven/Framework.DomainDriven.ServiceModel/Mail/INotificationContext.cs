using System.Net.Mail;

namespace Framework.DomainDriven.ServiceModel;

/// <summary>
/// Контект для уведомления/нотификаций
/// </summary>
public interface INotificationContext
{
    MailAddress SystemSender { get; }
}
