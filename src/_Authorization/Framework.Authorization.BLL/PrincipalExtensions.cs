using Anch.SecuritySystem.ExternalSystem.Management;

using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public static class PrincipalExtensions
{
    public static PrincipalData<Principal, Permission, PermissionRestriction> ToPrincipalData(this Principal principal) =>
        new(principal, principal.Permissions.Select(p => p.ToPermissionData()).ToArray());

    public static PermissionData<Permission, PermissionRestriction> ToPermissionData(this Permission permission) =>
        new(permission, permission.Restrictions.ToArray());
}

