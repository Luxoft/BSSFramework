using System.Collections.Immutable;

using Framework.Subscriptions.Domain;

using SecuritySystem;

namespace Framework.Subscriptions.Metadata;

public abstract class SubscriptionMetadata<TDomainObject, TSubscription, TMessageTemplate> : SubscriptionMetadata<TDomainObject, TDomainObject, TSubscription, TMessageTemplate>
    where TDomainObject : class
    where TSubscription : ISubscription<TDomainObject>
    where TMessageTemplate : IMessageTemplate<TDomainObject>;

public abstract class SubscriptionMetadata<TDomainObject, TRenderingObject, TSubscription, TMessageTemplate> : ISubscriptionMetadata
    where TDomainObject : class
    where TRenderingObject : class
    where TSubscription : ISubscription<TDomainObject, TRenderingObject>
    where TMessageTemplate : IMessageTemplate<TRenderingObject>
{
    /// <inheritdoc />
    public Type DomainObjectType { get; } = typeof(TDomainObject);

    public Type SubscriptionType { get; } = typeof(TSubscription);

    public Type MessageTemplateType { get; } = typeof(TMessageTemplate);

    /// <inheritdoc />
    public string Name => this.GetType().FullName!;

    public virtual DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Any;

    /// <inheritdoc />
    public virtual string? SenderName { get; } = null;

    /// <inheritdoc />
    public virtual string? SenderEmail { get; } = null;

    /// <inheritdoc />
    public virtual ImmutableArray<SecurityRole> SecurityRoles { get; } = [];

    /// <inheritdoc />
    public virtual RecipientMergeType RecipientMergeType { get; } = RecipientMergeType.Union;

    /// <inheritdoc />
    public virtual bool SendIndividualLetters { get; } = false;

    /// <inheritdoc />
    public virtual bool ExcludeCurrentUser { get; } = false;

    /// <inheritdoc />
    public virtual bool IncludeAttachments { get; } = true;

    /// <inheritdoc />
    public virtual bool AllowEmptyListOfRecipients { get; } = false;
}
