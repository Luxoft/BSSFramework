using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailablePermissionSource
{
    AvailablePermissionFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    IQueryable<Permission> GetAvailablePermissionsQueryable(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter);
}
