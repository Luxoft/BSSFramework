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
///     Defines the interface of a Code first subscription metadata instance.
/// </summary>
public interface ISubscription
{
    /// <summary>
    ///     Gets the domain object type of the subscription.
    /// </summary>
    /// <value>
    ///     Domain object type.
    /// </value>
    Type DomainObjectType { get; }

    Type RenderingObjectType { get; }

    SubscriptionHeader Header { get; }

    string MessageTemplateCode { get; }


    MailAddress? Sender { get; }

    /// <summary>
    ///     Gets a flag indicating whether an individual or a consolidated letter is sent to the addressee(s)
    /// </summary>
    /// <value>
    ///     <c>true</c> if individual letters must be sent; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     With <c>true</c>, instead of one letter for all addressees, a separate letter is formed for each
    ///     a delivery addressee (To and Copy), keeping its role. ReplyTo addressees are not delivery addressees
    ///     and are added to every formed letter.
    /// </remarks>
    bool SendIndividualLetters { get; }

    /// <summary>
    ///     Gets a flag indicating that attachments from the template must be attached to the letter.
    /// </summary>
    /// <value>
    ///     <c>true</c> if attachments must be attached; otherwise, <c>false</c>.
    /// </value>
    bool InlineAttachments { get; }

    /// <summary>
    ///     Gets a flag indicating that the notification can be sent with an empty list of recipients
    ///     (for example, when testing).
    /// </summary>
    /// <value>
    ///     <c>true</c> if the notification can be sent with an empty list of recipients;
    ///     otherwise, <c>false</c>.
    /// </value>
    bool AllowEmptyListOfRecipients { get; }

    DomainObjectChangeType DomainObjectChangeType { get; }

    /// <summary>
    ///     Gets the way recipients are combined by SubBusinessRoles and Generation.
    /// </summary>
    /// <value>
    ///     The way recipients are combined by SubBusinessRoles and Generation..
    /// </value>
    RecipientMergeType RecipientMergeType { get; }

    /// <summary>
    ///     Gets the tree permission extension type.
    /// </summary>
    /// <value>
    ///     Tree permission extension type.
    /// </value>

    /// <summary>
    ///     Gets business role identifiers.
    /// </summary>
    /// <value>
    ///     Business role identifiers..
    /// </value>
    DomainSecurityRule.RoleBaseSecurityRule? SecurityRule { get; }
}
