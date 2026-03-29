using Framework.Application.Repository;
using Framework.Configuration.Domain;
using Framework.Core.Helpers;
using Framework.Core.MessageSender;
using Framework.Notification.DTO;

using SecuritySystem.Attributes;

namespace Framework.Configuration.BLL.SubscriptionSystemService;

/// <summary>
/// Sender для отправки нотификакий в локальную бд
/// </summary>
public class LocalDBNotificationEventDTOMessageSender([DisabledSecurity] IRepository<DomainObjectNotification> domainObjectNotificationRepository) : IMessageSender<NotificationEventDTO>
{
    /// <inheritdoc />
    public async Task SendAsync(NotificationEventDTO dto, CancellationToken cancellationToken)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var serializedData = DataContractSerializerHelper.Serialize(dto);
        var dbNotification = new DomainObjectNotification
                             {
                                     SerializeData = serializedData,
                                     Size = serializedData.Length
                             };

        await domainObjectNotificationRepository.SaveAsync(dbNotification, cancellationToken);
    }
}
