using System.Collections.Immutable;
using System.Net.Mail;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Notification.Domain;

using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public interface ISubscription<TDomainObject, TRenderingObject> : ISubscription
    where TDomainObject : class
    where TRenderingObject : class
{
    Type ISubscription.DomainObjectType => typeof(TDomainObject);

    Type ISubscription.RenderingObjectType => typeof(TRenderingObject);

    ValueTask<bool> IsProcessed(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions, CancellationToken ct);

    ValueTask<TRenderingObject> ConvertToRenderingObject(IServiceProvider serviceProvider, TDomainObject domainObject, CancellationToken ct);

    IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetCopyTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetReplyTo(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    IAsyncEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(IServiceProvider serviceProvider, DomainObjectVersions<TDomainObject> versions);

    ValueTask<(string Subject, string Body)> GetMessage(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions, CancellationToken ct);

    IAsyncEnumerable<Attachment> GetAttachments(IServiceProvider serviceProvider, DomainObjectVersions<TRenderingObject> versions);
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

    Type RenderingObjectType { get; }

    SubscriptionHeader Header { get; }

    string MessageTemplateCode { get; }


    MailAddress Sender { get; }

    ///// <summary>
    /////     Получает признак необходимости отправки адресату(ам) индивидуального или консолидированного письма
    ///// </summary>
    ///// <value>
    /////     <c>true</c> если необходимо отправлять индивидуальные письма; в противном случае, <c>false</c>.
    ///// </value>
    //bool SendIndividualLetters { get; }

    /// <summary>
    ///     Получает признак того, что к письму необходимо прикрепить вложения из шаблона.
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо прикрепить вложения; в противном случае, <c>false</c>.
    /// </value>
    bool InlineAttachments { get; }

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

