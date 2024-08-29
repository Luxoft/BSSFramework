using Framework.Core;
using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationAvailableSecurityRoleSource(IAvailablePermissionSource availablePermissionSource, ISecurityRoleSource securityRoleSource)
{
    public async Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken)
    {
        var dbRequest = from permission in availablePermissionSource.GetAvailablePermissionsQueryable()

                        select permission.Role.Id;

        var dbRolesIdents = await dbRequest.Distinct().ToListAsync(cancellationToken);

        return dbRolesIdents.Select(securityRoleSource.GetSecurityRole);
    }
}
