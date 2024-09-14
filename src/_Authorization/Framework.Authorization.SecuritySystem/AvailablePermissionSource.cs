using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource(
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    TimeProvider timeProvider,
    ICurrentPrincipalSource currentPrincipalSource,
    ISecurityRolesIdentsResolver securityRolesIdentsResolver,
    SecurityRuleCredential defaultSecurityRuleCredential)
    : IAvailablePermissionSource
{
    public AvailablePermissionFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return new AvailablePermissionFilter(timeProvider.GetToday())
               {
                   PrincipalName = this.GetPrincipalName(securityRule.CustomCredential ?? defaultSecurityRuleCredential),
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

    private string? GetPrincipalName(SecurityRuleCredential securityRuleCredential)
    {
        switch (securityRuleCredential)
        {
            case SecurityRuleCredential.CustomPrincipalSecurityRuleCredential customPrincipalSecurityRuleCredential:
                return customPrincipalSecurityRuleCredential.Name;

            case { } when securityRuleCredential == SecurityRuleCredential.CurrentUserWithRunAs:
                return (currentPrincipalSource.CurrentPrincipal.RunAs ?? currentPrincipalSource.CurrentPrincipal).Name;

            case { } when securityRuleCredential == SecurityRuleCredential.CurrentUserWithoutRunAs:
                return currentPrincipalSource.CurrentPrincipal.Name;

            case { } when securityRuleCredential == SecurityRuleCredential.AnyUser:
                return null;

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRuleCredential));
        }
    }
}
