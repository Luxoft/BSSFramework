using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AvailableSecurityRoleSource(IAvailablePermissionSource availablePermissionSource, ISecurityRoleSource securityRoleSource)
    : IAvailableSecurityRoleSource
{
    public async Task<List<FullSecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken)
    {
        var dbRequest = from permission in availablePermissionSource.GetAvailablePermissionsQueryable()

                        select permission.Role.Id;

        var dbRolesIdents = await dbRequest.Distinct().ToListAsync(cancellationToken);

        return dbRolesIdents.Select(securityRoleSource.GetSecurityRole).ToList();
    }
}
