using System.Collections.Immutable;
using System.Net.Mail;

using Framework.Subscriptions.Domain;

using SecuritySystem;

namespace Framework.Subscriptions.Metadata;

public interface ISubscription<TDomainObject, TRenderingObject> : ISubscription<TDomainObject>
    where TDomainObject : class
    where TRenderingObject : class
{
    TRenderingObject ConvertToRenderingObject(IServiceProvider serviceProvider, TDomainObject domainObject);

    IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetCopyTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetReplyTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    IEnumerable<Attachment> GetAttachments(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    IEnumerable<TypedNotificationFilterGroup> GetNotificationFilterGroups(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    (string Subject, string Body) GetMessage(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions);
}

public interface ISubscription<TDomainObject> : ISubscription
    where TDomainObject : class
{
    Type ISubscription.DomainObjectType => typeof(TDomainObject);

    bool IsProcessed(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);
}

/// <summary>
///     Определяет интерфейс экземпляра метаданных Code first подписки.
/// </summary>
public interface ISubscription
{
    /// <summary>
    ///     Получает тип доменного объекта подписки.
    /// </summary>
    /// <value>
    ///     Тип доменного объекта.
    /// </value>
    Type DomainObjectType { get; }

    SubscriptionHeader Header { get; }

    /// <summary>
    ///     Получает имя отправителя уведомления по подписке.
    /// </summary>
    /// <value>
    ///     Имя отправителя уведомления по подписке.
    /// </value>
    string? SenderName { get; }

    /// <summary>
    ///     Получает адрес электронной почты отправителя уведомления по подписке.
    /// </summary>
    /// <value>
    ///     Адрес электронной почты отправителя уведомления по подписке.
    /// </value>
    string? SenderEmail { get; }

    /// <summary>
    ///     Получает признак необходимости отправки адресату(ам) индивидуального или консолидированного письма
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо отправлять индивидуальные письма; в противном случае, <c>false</c>.
    /// </value>
    bool SendIndividualLetters { get; }

    /// <summary>
    ///     Получает признак исключения пользователя из текущей рассылки.
    /// </summary>
    /// <value>
    ///     <c>true</c> если пользователя необходимо исключить; в противном случае, <c>false</c>.
    /// </value>
    bool ExcludeCurrentUser { get; }

    /// <summary>
    ///     Получает признак того, что к письму необходимо прикрепить вложения из шаблона.
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо прикрепить вложения; в противном случае, <c>false</c>.
    /// </value>
    bool IncludeAttachments { get; }

    /// <summary>
    ///     Получае признак того, что уведомление можно отправлять с пустым списком получателей
    ///     (например, в случае тестирования).
    /// </summary>
    /// <value>
    ///     <c>true</c> если уведомление можно отправлять с пустым списком получателей;
    ///     в противном случае, <c>false</c>.
    /// </value>
    bool AllowEmptyListOfRecipients { get; }

    DomainObjectChangeType DomainObjectChangeType { get; }

    /// <summary>
    ///     Получает способ комбинации адресатов рассылки по SubBusinessRoles и Generation.
    /// </summary>
    /// <value>
    ///     Способ комбинации адресатов рассылки по SubBusinessRoles и Generation..
    /// </value>
    RecipientMergeType RecipientMergeType { get; }

    /// <summary>
    ///     Получает тип расширения прав по дереву.
    /// </summary>
    /// <value>
    ///     Тип расширения прав по дереву.
    /// </value>

    /// <summary>
    ///     Получает идентификаторы бизнес-ролей.
    /// </summary>
    /// <value>
    ///     Идентификаторы бизнес-ролей..
    /// </value>
    ImmutableArray<SecurityRole> SecurityRoles { get; }
}
