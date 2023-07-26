using System.Linq.Expressions;

using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

public interface INotificationBasePermissionFilterSource
{
    Expression<Func<Permission, bool>> GetBasePermissionFilter(Guid[] roleIdents);
}
