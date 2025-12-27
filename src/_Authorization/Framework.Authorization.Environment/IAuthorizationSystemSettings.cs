using Framework.Authorization.Domain;
using Framework.Authorization.Notification;

using SecuritySystem.GeneralPermission.Validation;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemSettings
{
    bool RegisterRunAsManager { get; set; }

    IAuthorizationSystemSettings SetNotificationPermissionExtractor<T>()
        where T : INotificationPermissionExtractor;

    IAuthorizationSystemSettings SetUniquePermissionComparer<TComparer>()
        where TComparer : class, IPermissionEqualityComparer<Permission, PermissionRestriction>;
}
