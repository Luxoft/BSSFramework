using Framework.Authorization.Domain;

using SecuritySystem.GeneralPermission.Validation;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemSettings
{
    bool RegisterRunAsManager { get; set; }
    IAuthorizationSystemSettings SetUniquePermissionComparer<TComparer>()
        where TComparer : class, IPermissionEqualityComparer<Permission, PermissionRestriction>;
}
