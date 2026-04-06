using System.Collections.Immutable;

using SecuritySystem;
using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions;

public interface IEmployeeEmailExtractor
{
    ImmutableHashSet<string> GetEmails(ImmutableArray<SecurityRole> securityRoles, ImmutableArray<NotificationFilterGroup> notificationFilterGroups);
}
