using System.Net.Mail;

using Anch.SecuritySystem;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.Models.Custom;
using SampleSystem.Security;

namespace SampleSystem.Subscriptions.Metadata.Examples.SecurityRuleNoFilter;

/// <summary>
/// Subscription with SecurityRule but without GetNotificationFilterGroups: recipients are all employees whose principals have the role on any context.
/// Empty list of recipients is allowed, so the letter must fall back to the support address.
/// </summary>
public class SecurityRuleNoFilterSubscription : Subscription<NoRecipientModel, SecurityRuleNoFilterTemplate>
{
    public override MailAddress Sender { get; } = new("SecurityRuleNoFilter@luxoft.com", "SecurityRuleNoFilter");

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override bool AllowEmptyListOfRecipients { get; } = true;

    public override DomainSecurityRule.RoleBaseSecurityRule SecurityRule { get; } =
        new DomainSecurityRule.ExpandedRolesSecurityRule([SampleSystemSecurityRole.SearchTestBusinessRole]);
}
