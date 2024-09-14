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
    DefaultSecurityRuleCredentialInfo defaultSecurityRuleCredentialInfo)
    : IAvailablePermissionSource
{
    public AvailablePermissionFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var credential = securityRule.CustomCredential ?? defaultSecurityRuleCredentialInfo.Credential;

        return new AvailablePermissionFilter(timeProvider.GetToday())
               {
                   PrincipalName = this.GetPrincipalName(credential),
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

    private string? GetPrincipalName(SecurityRuleCredential credential)
    {
        switch (credential)
        {
            case SecurityRuleCredential.CustomPrincipalSecurityRuleCredential customPrincipalSecurityRuleCredential:
                return customPrincipalSecurityRuleCredential.Name;

            case { } when credential == SecurityRuleCredential.CurrentUserWithRunAs:
                return (currentPrincipalSource.CurrentPrincipal.RunAs ?? currentPrincipalSource.CurrentPrincipal).Name;

            case { } when credential == SecurityRuleCredential.CurrentUserWithoutRunAs:
                return currentPrincipalSource.CurrentPrincipal.Name;

            case { } when credential == SecurityRuleCredential.AnyUser:
                return null;

            default:
                throw new ArgumentOutOfRangeException(nameof(credential));
        }
    }
}
