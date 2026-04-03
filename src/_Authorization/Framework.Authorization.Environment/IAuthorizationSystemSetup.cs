using Framework.Authorization.Domain;

using SecuritySystem.GeneralPermission.Validation;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemSetup
{
    bool RegisterRunAsManager { get; set; }

    IAuthorizationSystemSetup SetUniquePermissionComparer<TComparer>()
        where TComparer : class, IPermissionEqualityComparer<Permission, PermissionRestriction>;
}
