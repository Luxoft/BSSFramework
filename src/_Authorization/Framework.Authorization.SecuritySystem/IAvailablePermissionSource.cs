using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailablePermissionSource
{
    IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true, DomainSecurityRule.RoleBaseSecurityRule? securityRule = null, bool applyCurrentUser = true);

    IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter);
}
