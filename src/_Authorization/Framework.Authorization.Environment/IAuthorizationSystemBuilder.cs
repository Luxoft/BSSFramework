using Framework.Authorization.Domain;

using SecuritySystem.GeneralPermission.Validation;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemBuilder
{
    bool RegisterRunAsManager { get; set; }

    IAuthorizationSystemBuilder SetUniquePermissionComparer<TComparer>()
        where TComparer : class, IPermissionEqualityComparer<Permission, PermissionRestriction>;
}
