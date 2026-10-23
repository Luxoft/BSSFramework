using System.Net.Mail;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Notification.Domain;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Models.Custom;
using SampleSystem.Security;

namespace SampleSystem.Subscriptions.Metadata.Examples.SecurityRule;

/// <summary>
/// Пример подписки, получатели которой определяются через SecurityRule + GetNotificationFilterGroups:
/// письмо получают сотрудники, чьи принципалы имеют роль SearchTestBusinessRole на BusinessUnit с идентификатором <see cref="BusinessUnitId" />.
/// </summary>
public class SecurityRuleSubscription : Subscription<NoRecipientModel, SecurityRuleTemplate>
{
    public static readonly Guid BusinessUnitId = new("6B1F6C0E-6B0B-4B53-9A38-2C7D0C5E8F11");

    public override MailAddress Sender { get; } = new("SecurityRule@luxoft.com", "SecurityRule");

    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override DomainSecurityRule.RoleBaseSecurityRule SecurityRule { get; } =
        new DomainSecurityRule.ExpandedRolesSecurityRule([SampleSystemSecurityRole.SearchTestBusinessRole]);

    public override async IAsyncEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(
        IServiceProvider serviceProvider,
        DomainObjectVersions<NoRecipientModel> versions)
    {
        yield return new NotificationFilterGroup<Guid>
                     {
                         SecurityContextType = typeof(BusinessUnit),
                         ExpandType = NotificationExpandType.Direct,
                         Idents = [BusinessUnitId]
                     };

        await Task.CompletedTask;
    }
}
