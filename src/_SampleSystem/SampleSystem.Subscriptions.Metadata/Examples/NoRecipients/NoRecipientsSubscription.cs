using System.Net.Mail;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.Examples.NoRecipients;

/// <summary>
/// Пример подписки без единого получателя: GetTo/GetCopyTo/GetReplyTo/SecurityRoles не переопределены,
/// поэтому все три источника получателей пусты.
/// Воспроизводит проблему: когда получателей нет совсем, уведомление вообще не формируется
/// (NotificationExtractor группирует получателей по ключу DomainObjectVersions, и при пустых
/// источниках такой ключ просто не появляется), поэтому модификатор RedirectToSupportMailMessageModifier,
/// который должен подставлять адрес поддержки в пустое поле To, не успевает сработать - письмо не уходит никуда.
/// </summary>
public class NoRecipientsSubscription : Subscription<NoRecipientModel, NoRecipientsTemplate>
{
    public override MailAddress Sender { get; } = new("NoRecipients@luxoft.com", "NoRecipients");

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override bool AllowEmptyListOfRecipients { get; } = true;
}
