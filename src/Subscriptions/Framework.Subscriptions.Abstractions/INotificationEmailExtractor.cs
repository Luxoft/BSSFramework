using System.Collections.Immutable;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions;

public interface INotificationEmailExtractor
{
    IAsyncEnumerable<string> GetEmails(DomainSecurityRule.RoleBaseSecurityRule securityRule, ImmutableArray<NotificationFilterGroup> notificationFilterGroups);
}
