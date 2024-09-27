using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource(
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    IUserNameResolver userNameResolver,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver,
    SecurityRuleCredential defaultSecurityRuleCredential)
    : IAvailablePermissionSource
{
    public AvailablePermissionFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return new AvailablePermissionFilter(timeProvider.GetToday())
               {
                   PrincipalName = userNameResolver.Resolve(securityRule.CustomCredential ?? defaultSecurityRuleCredential),
                   SecurityRoleIdents = securityRolesIdentsResolver.Resolve(securityRule).ToList()
               };
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var filter = this.CreateFilter(securityRule);

        return this.GetAvailablePermissionsQueryable(filter);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
    {
        return permissionRepository.GetQueryable().Where(filter.ToFilterExpression());
    }
}
