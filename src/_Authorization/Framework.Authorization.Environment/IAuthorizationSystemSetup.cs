using Anch.SecuritySystem.GeneralPermission.Validation;

using Framework.Authorization.Domain;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemSetup
{
    bool RegisterRunAsManager { get; set; }

    IAuthorizationSystemSetup SetUniquePermissionComparer<TComparer>()
        where TComparer : class, IPermissionEqualityComparer<Permission, PermissionRestriction>;
}

