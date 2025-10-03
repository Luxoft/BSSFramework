using System.Net.Mail;

using Framework.Notification;
using SecuritySystem;

namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
/// Базовый класс экземпляра метаданных Code first подписки для случая с переопределением доменной модели при генерации получателей.
/// </summary>
/// <typeparam name="TContext">Текущий контекст бизнес-логики.</typeparam>
/// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
/// <typeparam name="TCustomObject">Тип объекта, который подменяет TDomainObject при генерации получателей.</typeparam>
/// <typeparam name="TTemplate">Тип Razor шаблона.</typeparam>
/// <seealso cref="ISubscriptionMetadata" />
public abstract class SubscriptionWithCustomModelMetadata<TContext, TDomainObject, TCustomObject, TTemplate> : ISubscriptionMetadata
        where TDomainObject : class
        where TCustomObject : class
        where TTemplate : IRazorTemplate
{
    /// <inheritdoc />
    public string Code => this.GetType().FullName;

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Condition.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Condition.
    /// </value>
    public virtual LambdaMetadata<TContext, TDomainObject, bool> ConditionLambda { get; protected set; }

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Generation.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Generation.
    /// </value>
    public virtual LambdaMetadata<TContext, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>> GenerationLambda { get; protected set; }

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Generation.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Generation.
    /// </value>
    public virtual LambdaMetadata<TContext, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>> CopyGenerationLambda { get; protected set; }

    /// <summary>
    ///     Получает экземпляр лямбда-выражения Generation для определение replyTo.
    /// </summary>
    /// <value>
    ///     Экземпляр лямбда-выражения Generation.
    /// </value>
    public virtual LambdaMetadata<TContext, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>> ReplyToGenerationLambda { get; protected set; }


    /// <summary>
    ///     Получает экземпляр лямбда-выражения Attachment.
    /// </summary>
    public virtual LambdaMetadata<TContext, TCustomObject, IEnumerable<Attachment>> AttachmentLambda { get; protected set; }

    /// <summary>
    ///     Получает коллекцию экземпляров лямбда-выражения SecurityItemSource.
    /// </summary>
    /// <value>
    ///     Коллекция экземпляров лямбда-выражения SecurityItemSource.
    /// </value>
    public virtual
            IEnumerable<ISecurityItemSourceLambdaMetadata<TContext, TDomainObject, ISecurityContext>>
            SecurityItemSourceLambdas
    { get; protected set; }

    /// <inheritdoc />
    public virtual string SenderName { get; protected set; }

    /// <inheritdoc />
    public virtual string SenderEmail { get; protected set; }

    /// <inheritdoc />
    public virtual IEnumerable<SecurityRole> SubBusinessRoles { get; protected set; } = new List<SecurityRole>();

    /// <inheritdoc />
    public virtual RecepientsSelectorMode RecepientsSelectorMode { get; protected set; } = RecepientsSelectorMode.Union;

    /// <inheritdoc />
    public virtual bool SendIndividualLetters { get; protected set; } = false;

    /// <inheritdoc />
    public virtual bool ExcludeCurrentUser { get; protected set; } = false;

    /// <inheritdoc />
    public virtual bool IncludeAttachments { get; protected set; } = true;

    /// <inheritdoc />
    public virtual bool AllowEmptyListOfRecipients { get; protected set; } = false;

    /// <inheritdoc />
    public Type MetadataType => this.GetType();

    /// <inheritdoc />
    public Type DomainObjectType { get; } = typeof(TDomainObject);

    /// <inheritdoc />
    public Type MessageTemplateType { get; } = typeof(TTemplate);

    /// <inheritdoc />
    public ILambdaMetadata GetConditionLambda()
    {
        return this.ConditionLambda;
    }

    /// <inheritdoc />
    public ILambdaMetadata GetGenerationLambda()
    {
        return this.GenerationLambda;
    }

    /// <inheritdoc />
    public ILambdaMetadata GetAttachmentLambda()
    {
        return this.AttachmentLambda;
    }

    /// <inheritdoc />
    public ILambdaMetadata GetCopyGenerationLambda()
    {
        return this.CopyGenerationLambda;
    }

    public ILambdaMetadata GetReplyToGenerationLambda() => this.ReplyToGenerationLambda;

    /// <inheritdoc />
    public IEnumerable<ISecurityItemSourceLambdaMetadata> GetSecurityItemSourceLambdas()
    {
        return this.SecurityItemSourceLambdas;
    }

    /// <inheritdoc />
    public void Validate()
    {
        this.ValidateString(this.SenderName, nameof(this.SenderName));
        this.ValidateString(this.SenderEmail, nameof(this.SenderEmail));

        this.ValidateObject(this.ConditionLambda, nameof(this.ConditionLambda));

        this.ConditionLambda.Validate();
        this.GenerationLambda?.Validate();
        this.CopyGenerationLambda?.Validate();

        if (this.SecurityItemSourceLambdas != null)
        {
            foreach (var lambda in this.SecurityItemSourceLambdas)
            {
                lambda.Validate();
            }
        }
    }

    private void ValidateString(string propertyValue, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            throw new SubscriptionModelingException($"Property '{propertyName}' of subscription '{this.GetType().FullName}' could not be null or whitespace.");
        }
    }

    private void ValidateObject(object propertyValue, string propertyName)
    {
        if (propertyValue == null)
        {
            throw new SubscriptionModelingException($"Property '{propertyName}' of subscription '{this.GetType().FullName}' must be specified.");
        }
    }
}
