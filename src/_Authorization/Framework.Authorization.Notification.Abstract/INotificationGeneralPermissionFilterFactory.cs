using System.Linq.Expressions;

using Framework.Authorization.Domain;

using SecuritySystem;

namespace Framework.Authorization.Notification;

public interface INotificationGeneralPermissionFilterFactory
{
    Expression<Func<Permission, bool>> Create(IEnumerable<SecurityRole> securityRoles);
}
