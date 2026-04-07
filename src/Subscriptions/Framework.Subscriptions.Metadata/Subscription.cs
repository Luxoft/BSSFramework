using System.Collections.Immutable;
using System.Net.Mail;

using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions.Metadata;


public abstract class Subscription<TDomainObject, TMessageTemplate> : Subscription<TDomainObject, TDomainObject, TMessageTemplate>
    where TDomainObject : class
    where TMessageTemplate : IMessageTemplate<TDomainObject>
{
    public sealed override TDomainObject ConvertToRenderingObject(IServiceProvider serviceProvider, TDomainObject domainObject) => domainObject;
}

public abstract class Subscription<TDomainObject, TRenderingObject, TMessageTemplate> : ISubscription<TDomainObject, TRenderingObject>
    where TDomainObject : class
    where TRenderingObject : class
    where TMessageTemplate : IMessageTemplate<TRenderingObject>
{
    public abstract DomainObjectChangeType DomainObjectChangeType { get; }

    public virtual SubscriptionHeader Header => new(this.GetType().FullName!);

    public abstract MailAddress Sender { get; }

    public virtual bool SendIndividualLetters { get; } = false;

    public virtual bool ExcludeCurrentUser { get; } = false;

    public virtual bool IncludeAttachments { get; } = true;

    public virtual bool AllowEmptyListOfRecipients { get; } = false;

    public virtual RecipientMergeType RecipientMergeType { get; } = RecipientMergeType.Union;

    public virtual ImmutableArray<SecurityRole> SecurityRoles { get; } = [];

    public virtual bool IsProcessed(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions) => true;

    public abstract TRenderingObject ConvertToRenderingObject(IServiceProvider serviceProvider, TDomainObject domainObject);

    public virtual IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions) => [];

    public virtual IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetCopyTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions) => [];

    public virtual IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetReplyTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions) =>
        [];

    public virtual IEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions) => [];

    public (string Subject, string Body) GetMessage(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions) =>
        serviceProvider.GetRequiredService<TMessageTemplate>().Render(serviceProvider, versions);

    public virtual IEnumerable<Attachment> GetAttachments(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions) => [];
}
