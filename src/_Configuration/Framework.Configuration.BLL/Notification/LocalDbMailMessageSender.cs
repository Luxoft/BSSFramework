using System.Net.Mail;

using Framework.Application.Repository;
using Framework.Configuration.Domain;
using Framework.Core.Helpers;
using Framework.Core;
using Framework.Notification.DTO;

using SecuritySystem.Attributes;

namespace Framework.Configuration.BLL.Notification;

/// <summary>
/// Sender для отправки нотификакий в локальную бд
/// </summary>
public class LocalDbMailMessageSender([DisabledSecurity] IRepository<DomainObjectNotification> domainObjectNotificationRepository)
    : IMessageSender<MailMessage>
{
    /// <inheritdoc />
    public async Task SendAsync(MailMessage mailMessage, CancellationToken cancellationToken)
    {
        var dto = new NotificationEventDTO(mailMessage);

        var serializedData = DataContractSerializerHelper.Serialize(dto);
        var dbNotification = new DomainObjectNotification { SerializeData = serializedData, Size = serializedData.Length };

        await domainObjectNotificationRepository.SaveAsync(dbNotification, cancellationToken);
    }
}
