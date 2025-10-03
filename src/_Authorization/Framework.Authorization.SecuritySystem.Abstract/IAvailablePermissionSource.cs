using Framework.Authorization.Domain;
using SecuritySystem;

namespace Framework.Authorization.SecuritySystemImpl;

public interface IAvailablePermissionSource
{
    AvailablePermissionFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    IQueryable<Permission> GetAvailablePermissionsQueryable(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter);
}
