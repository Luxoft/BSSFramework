using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.Notification;

public interface INotificationBasePermissionFilterSource
{
    Expression<Func<Permission, bool>> GetBasePermissionFilter(SecurityRole[] securityRoles);
}
