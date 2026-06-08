using System.Collections.Immutable;
using System.Net.Mail;

using Anch.Core;
using Anch.SecuritySystem;
using Anch.SecuritySystem.Notification.Domain;

using Framework.Subscriptions.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions.Metadata;

public abstract class Subscription<TDomainObject, TMessageTemplate> : Subscription<TDomainObject, TDomainObject, TMessageTemplate>
    where TDomainObject : class
    where TMessageTemplate : IMessageTemplate<TDomainObject>
{
    public sealed override ValueTask<TDomainObject> ConvertToRenderingObject(IServiceProvider serviceProvider, TDomainObject domainObject, CancellationToken ct) => new(domainObject);
}

public abstract class Subscription<TDomainObject, TRenderingObject, TMessageTemplate> : ISubscription<TDomainObject, TRenderingObject>
    where TDomainObject : class
    where TRenderingObject : class
    where TMessageTemplate : IMessageTemplate<TRenderingObject>
{
    public abstract DomainObjectChangeType DomainObjectChangeType { get; }

    public virtual SubscriptionHeader Header => new(this.GetType().FullName!);

    public string MessageTemplateCode { get; } = typeof(TMessageTemplate).FullName!;

    public abstract MailAddress Sender { get; }

    //public virtual bool SendIndividualLetters { get; } = false;

    public virtual bool InlineAttachments { get; } = true;

    public virtual RecipientMergeType RecipientMergeType { get; } = RecipientMergeType.Union;

    public virtual ImmutableArray<SecurityRole> SecurityRoles { get; } = [];

    public ValueTask<(string Subject, string Body)> GetMessage(
        IServiceProvider serviceProvider,
        DomainObjectVersions<TRenderingObject> versions,
        CancellationToken ct) =>
        serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<TMessageTemplate>().Render(serviceProvider, versions, ct);

    public abstract ValueTask<TRenderingObject> ConvertToRenderingObject(IServiceProvider serviceProvider, TDomainObject domainObject, CancellationToken ct);

    public virtual ValueTask<bool> IsProcessed(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions, CancellationToken ct) =>
        new(true);

    public virtual IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<TDomainObject> versions) => AsyncEnumerable.Empty<NotificationMessageGenerationInfo<TRenderingObject>>();

    public virtual IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetCopyTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<TDomainObject> versions) => AsyncEnumerable.Empty<NotificationMessageGenerationInfo<TRenderingObject>>();

    public virtual IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetReplyTo(
        IServiceProvider serviceProvider,
        DomainObjectVersions<TDomainObject> versions) =>
        AsyncEnumerable.Empty<NotificationMessageGenerationInfo<TRenderingObject>>();

    public virtual IAsyncEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(
        IServiceProvider serviceProvider,
        DomainObjectVersions<TDomainObject> versions) => AsyncEnumerable.Empty<NotificationFilterGroup>();

    public virtual IAsyncEnumerable<Attachment> GetAttachments(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions) =>
        AsyncEnumerable.Empty<Attachment>();
}
