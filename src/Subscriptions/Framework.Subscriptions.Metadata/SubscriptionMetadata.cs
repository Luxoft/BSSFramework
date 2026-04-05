using System.Collections.Immutable;
using System.Net.Mail;

using Framework.Subscriptions.Domain;

using SecuritySystem;

namespace Framework.Subscriptions.Metadata;

public abstract class SubscriptionMetadata<TDomainObject, TMessageTemplate> : SubscriptionMetadata<TDomainObject, TDomainObject, TMessageTemplate>
    where TDomainObject : class
    where TMessageTemplate : IMessageTemplate<TDomainObject>;

public abstract class SubscriptionMetadata<TDomainObject, TRenderingObject, TMessageTemplate> : SubscriptionMetadata<TDomainObject>
    where TDomainObject : class
    where TMessageTemplate : IMessageTemplate<TRenderingObject>
{
    public sealed override Type MessageTemplateType { get; } = typeof(TMessageTemplate);
}

public abstract class SubscriptionMetadata<TDomainObject> : ISubscriptionMetadata
    where TDomainObject : class
{
    public abstract Type MessageTemplateType { get; }

    /// <inheritdoc />
    public string Name => this.GetType().FullName!;

    public virtual DomainObjectChangeType DomainObjectChangeType { get; protected init; } = DomainObjectChangeType.Any;

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Condition.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Condition.
    /// </value>
    public virtual LambdaMetadata<TDomainObject, bool> ConditionLambda { get; protected init; } = LambdaMetadata<TDomainObject>.Create((service, versions) => true);

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Generation.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Generation.
    /// </value>
    public virtual LambdaMetadata<TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>? GenerationLambda { get; protected init; }

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Generation.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Generation.
    /// </value>
    public virtual LambdaMetadata<TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>? CopyGenerationLambda { get; protected init; }

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Generation для определение replyTo.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Generation.
    /// </value>
    public virtual LambdaMetadata<TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>? ReplyToGenerationLambda { get; protected init; }


    /// <summary>
    ///     Получает экземпляр лямбда-выражения Attachment.
    /// </summary>
    public virtual LambdaMetadata<TDomainObject, IEnumerable<Attachment>>? AttachmentLambda { get; protected init; }

    /// <summary>
    ///     Получает коллекцию экземпляров лямбда-выражения SecurityItemSource.
    /// </summary>
    /// <value>
    ///     Коллекция экземпляров лямбда-выражения SecurityItemSource.
    /// </value>
    public virtual ImmutableArray<ISecurityItemSourceLambdaMetadata> SecurityItemSourceLambdas { get; protected init; } = [];

    /// <inheritdoc />
    public virtual string? SenderName { get; protected init; }

    /// <inheritdoc />
    public virtual string? SenderEmail { get; protected init; }

    /// <inheritdoc />
    public virtual ImmutableArray<SecurityRole> SecurityRoles { get; protected init; } = [];

    /// <inheritdoc />
    public virtual RecipientsSelectorMode RecipientsSelectorMode { get; protected init; } = RecipientsSelectorMode.Union;

    /// <inheritdoc />
    public virtual bool SendIndividualLetters { get; protected init; } = false;

    /// <inheritdoc />
    public virtual bool ExcludeCurrentUser { get; protected init; } = false;

    /// <inheritdoc />
    public virtual bool IncludeAttachments { get; protected init; } = true;

    /// <inheritdoc />
    public virtual bool AllowEmptyListOfRecipients { get; protected init; } = false;

    /// <inheritdoc />
    public Type DomainObjectType { get; } = typeof(TDomainObject);

    /// <inheritdoc />
    public ILambdaMetadata GetConditionLambda() => this.ConditionLambda;

    /// <inheritdoc />
    public ILambdaMetadata? GetGenerationLambda() => this.GenerationLambda;

    /// <inheritdoc />
    public ILambdaMetadata? GetAttachmentLambda() => this.AttachmentLambda;

    /// <inheritdoc />
    public ILambdaMetadata? GetCopyGenerationLambda() => this.CopyGenerationLambda;

    public ILambdaMetadata? GetReplyToGenerationLambda() => this.ReplyToGenerationLambda;

    /// <inheritdoc />
    public IEnumerable<ISecurityItemSourceLambdaMetadata> GetSecurityItems() => this.SecurityItemSourceLambdas;
}
