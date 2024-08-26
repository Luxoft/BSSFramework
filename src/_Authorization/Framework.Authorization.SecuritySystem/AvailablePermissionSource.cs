using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource(
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    ICurrentPrincipalSource currentPrincipalSource,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver)
    : IAvailablePermissionSource
{
    public IQueryable<Permission> GetAvailablePermissionsQueryable(
        DomainSecurityRule.RoleBaseSecurityRule? securityRule = null,
        bool applyCurrentUser = true,
        bool withRunAs = true)
    {
        var filter = new AvailablePermissionFilter(timeProvider.GetToday())
                     {
                         PrincipalName = this.GetPrincipalName(applyCurrentUser, withRunAs),
                         SecurityRoleIdents = securityRule == null ? null : securityRolesIdentsResolver.Resolve(securityRule).ToList()
                     };

        return this.GetAvailablePermissionsQueryable(filter);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
    {
        return permissionRepository.GetQueryable().Where(filter.ToFilterExpression());
    }

    private string? GetPrincipalName(bool applyCurrentUser, bool withRunAs)
    {
        if (applyCurrentUser)
        {
            var usedPrincipal = withRunAs
                                    ? currentPrincipalSource.CurrentPrincipal.RunAs ?? currentPrincipalSource.CurrentPrincipal
                                    : currentPrincipalSource.CurrentPrincipal;

            return usedPrincipal.Name;
        }
        else
        {
            return null;
        }
    }
}
