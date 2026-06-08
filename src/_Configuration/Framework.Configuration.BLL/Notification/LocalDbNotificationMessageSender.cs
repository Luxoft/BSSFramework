using Anch.SecuritySystem.Attributes;

using Framework.Application.Repository;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Helpers;
using Framework.Notification.DTO;

namespace Framework.Configuration.BLL.Notification;

/// <summary>
/// Sender для отправки нотификакий в локальную бд
/// </summary>
public class LocalDbNotificationMessageSender([DisabledSecurity] IRepository<DomainObjectNotification> domainObjectNotificationRepository)
    : IMessageSender<Framework.Notification.Domain.Notification>
{
    /// <inheritdoc />
    public async Task SendAsync(Framework.Notification.Domain.Notification notification, CancellationToken ct)
    {
        var dto = new NotificationEventDTO(notification);

        var serializedData = DataContractSerializerHelper.Serialize(dto);
        var dbNotification = new DomainObjectNotification { SerializeData = serializedData, Size = serializedData.Length };

        await domainObjectNotificationRepository.SaveAsync(dbNotification, ct);
    }
}

