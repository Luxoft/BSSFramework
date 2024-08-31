using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailablePermissionSource
{
    AvailablePermissionFilter CreateFilter(
        DomainSecurityRule.RoleBaseSecurityRule? securityRule = null,
        bool applyCurrentUser = true,
        bool withRunAs = true);

    IQueryable<Permission> GetAvailablePermissionsQueryable(DomainSecurityRule.RoleBaseSecurityRule? securityRule = null, bool applyCurrentUser = true, bool withRunAs = true);

    IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter);
}
