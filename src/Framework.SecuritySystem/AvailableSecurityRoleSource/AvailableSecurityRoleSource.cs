using Framework.Core;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public class AvailableSecurityRoleSource(IEnumerable<IPermissionSystem> permissionSystems, ISecurityRoleSource securityRoleSource)
    : IAvailableSecurityRoleSource
{
    public async Task<List<FullSecurityRole>> GetAvailableSecurityRoles(bool expandChildren, CancellationToken cancellationToken)
    {
        var allRoles = await Task.WhenAll(permissionSystems.Select(ps => ps.GetAvailableSecurityRoles(cancellationToken)));

        var roles = allRoles.SelectMany().Distinct().Select(securityRoleSource.GetSecurityRole);

        var rolesWithExpand =
            expandChildren
                ? roles.GetAllElements(sr => sr.Information.Children.Select(securityRoleSource.GetSecurityRole)).Distinct()
                : roles;

        return rolesWithExpand.ToList();
    }
}
