using Framework.Authorization.Domain;

using SecuritySystem;

namespace Framework.Authorization.Notification;

public interface INotificationPrincipalExtractor
{
    Task<IEnumerable<Principal>> GetPrincipalsAsync(SecurityRole[] securityRoles, IEnumerable<NotificationFilterGroup> notificationFilterGroups, CancellationToken cancellationToken = default);
}
