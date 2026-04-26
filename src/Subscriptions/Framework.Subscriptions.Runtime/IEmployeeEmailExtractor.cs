using System.Collections.Immutable;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions;

public interface IEmployeeEmailExtractor
{
    ImmutableHashSet<string> GetEmails(ImmutableArray<SecurityRole> securityRoles, ImmutableArray<NotificationFilterGroup> notificationFilterGroups);
}
