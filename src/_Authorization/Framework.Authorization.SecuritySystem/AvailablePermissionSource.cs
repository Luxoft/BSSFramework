using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource(
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    IActualPrincipalSource actualPrincipalSource,
    IUserAuthenticationService userAuthenticationService,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver)
    : IAvailablePermissionSource
{
    public IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true, DomainSecurityRule.RoleBaseSecurityRule? securityRule = null, bool applyCurrentUser = true)
    {
        var securityRoleIdents =
            securityRule == null
                ? null
                : securityRolesIdentsResolver.Resolve(securityRule).ToList();

        var filter = new AvailablePermissionFilter(timeProvider.GetToday())
                     {
                         PrincipalName = applyCurrentUser ? withRunAs ? actualPrincipalSource.ActualPrincipal.Name : userAuthenticationService.GetUserName() : null,
                         SecurityRoleIdents = securityRoleIdents
                     };

        return this.GetAvailablePermissionsQueryable(filter);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
    {
        return permissionRepository.GetQueryable().Where(filter.ToFilterExpression());
    }
}
