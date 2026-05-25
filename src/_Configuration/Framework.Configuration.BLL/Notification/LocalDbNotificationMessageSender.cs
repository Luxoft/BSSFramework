using Framework.Application.Repository;
using Framework.Configuration.Domain;
using Framework.Core.Helpers;
using Framework.Core;
using Framework.Notification.DTO;

using Anch.SecuritySystem.Attributes;

namespace Framework.Configuration.BLL.Notification;

/// <summary>
/// Sender для отправки нотификакий в локальную бд
/// </summary>
public class LocalDbNotificationMessageSender([DisabledSecurity] IRepository<DomainObjectNotification> domainObjectNotificationRepository)
    : IMessageSender<Framework.Notification.Domain.Notification>
{
    /// <inheritdoc />
    public async Task SendAsync(Framework.Notification.Domain.Notification notification, CancellationToken cancellationToken)
    {
        var dto = new NotificationEventDTO(notification);

        var serializedData = DataContractSerializerHelper.Serialize(dto);
        var dbNotification = new DomainObjectNotification { SerializeData = serializedData, Size = serializedData.Length };

        await domainObjectNotificationRepository.SaveAsync(dbNotification, cancellationToken);
    }
}
