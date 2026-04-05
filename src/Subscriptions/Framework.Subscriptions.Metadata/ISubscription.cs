using System.Net.Mail;

using Framework.Subscriptions.Domain;

using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions.Metadata;

public interface ISubscription<TDomainObject>
    where TDomainObject : class
{
    bool IsProcessed(DomainObjectVersions<TDomainObject> versions) => true;

    IEnumerable<NotificationMessageGenerationInfo> GetTo(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<NotificationMessageGenerationInfo> GetCopyTo(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<NotificationMessageGenerationInfo> GetReplyTo(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<Attachment> GetAttachments(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(DomainObjectVersions<TDomainObject> versions) => [];
}
