using System.Net.Mail;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.Examples.NoRecipients;

/// <summary>
/// Example of a subscription without a single recipient: GetTo/GetCopyTo/GetReplyTo/SecurityRoles are not overridden,
/// so all three recipient sources are empty.
/// Reproduces the problem: when there are no recipients at all, the notification is not formed at all
/// (NotificationExtractor groups recipients by the DomainObjectVersions key, and when empty
/// sources such a key simply does not appear), so the RedirectToSupportMailMessageModifier modifier,
/// which should substitute the support address into an empty To field, does not manage to run - the letter goes nowhere.
/// </summary>
public class NoRecipientsSubscription : Subscription<NoRecipientModel, NoRecipientsTemplate>
{
    public override MailAddress Sender { get; } = new("NoRecipients@luxoft.com", "NoRecipients");

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override bool AllowEmptyListOfRecipients { get; } = true;
}
