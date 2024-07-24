using Framework.Core;
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

        var dbRoles = dbRolesIdents.Select(securityRoleSource.GetSecurityRole);

        return dbRoles.GetAllElements(sr => sr.Information.Children.Select(securityRoleSource.GetSecurityRole))
                      .Distinct()
                      .ToList();
    }
}
