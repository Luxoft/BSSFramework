using System.Net.Mail;

using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public interface ISubscription<TDomainObject> : ISubscription<TDomainObject, TDomainObject>
    where TDomainObject : class
{
    TDomainObject ISubscription<TDomainObject, TDomainObject>.ConvertToRenderingObject(TDomainObject domainObject) => domainObject;
}

public interface ISubscription<TDomainObject, TRenderingObject>
    where TDomainObject : class
    where TRenderingObject : class
{
    TRenderingObject ConvertToRenderingObject(TDomainObject domainObject);

    bool IsProcessed(DomainObjectVersions<TDomainObject> versions) => true;

    IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetTo(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetCopyTo(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetReplyTo(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<Attachment> GetAttachments(DomainObjectVersions<TDomainObject> versions) => [];

    IEnumerable<TypedNotificationFilterGroup> GetNotificationFilterGroups(DomainObjectVersions<TDomainObject> versions) => [];
}
