using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailablePermissionSource
{
    IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true, List<Guid> securityRoleIdents = default, bool applyCurrentUser = true);

    IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter);
}
