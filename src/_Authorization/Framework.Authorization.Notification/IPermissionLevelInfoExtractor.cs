using System.Linq.Expressions;

namespace Framework.Authorization.Notification;

public interface IPermissionLevelInfoExtractor
{
    Expression<Func<PermissionLevelInfo, FullPermissionLevelInfo>> GetSelector(NotificationFilterGroup notificationFilterGroup);
}
