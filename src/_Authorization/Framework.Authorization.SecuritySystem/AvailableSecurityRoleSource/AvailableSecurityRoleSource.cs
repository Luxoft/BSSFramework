using GenericQueryable;

using SecuritySystem;

namespace Framework.Authorization.SecuritySystemImpl;

public class AuthorizationAvailableSecurityRoleSource(
    IAvailablePermissionSource availablePermissionSource,
    ISecurityRoleSource securityRoleSource,
    SecurityRuleCredential securityRuleCredential)
{
    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken)
    {
        var dbRequest =
            from permission in availablePermissionSource.GetAvailablePermissionsQueryable(
                DomainSecurityRule.AnyRole with { CustomCredential = securityRuleCredential })

            select permission.Role.Id;

        var dbRolesIdents = await dbRequest.Distinct().GenericToListAsync(cancellationToken);

        return dbRolesIdents.Select(securityRoleSource.GetSecurityRole);
    }
}
