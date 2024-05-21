using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AvailableSecurityRoleSource : IAvailableSecurityRoleSource
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly ISecurityRoleSource securityRoleSource;

    public AvailableSecurityRoleSource(IAvailablePermissionSource availablePermissionSource, ISecurityRoleSource securityRoleSource)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.securityRoleSource = securityRoleSource;
    }

    public async Task<List<FullSecurityRole>> GetAvailableSecurityRoles (CancellationToken cancellationToken)
    {
        var dbRequest = from permission in this.availablePermissionSource.GetAvailablePermissionsQueryable()

                        select permission.Role.Id;

        var dbOperationIdents = await dbRequest.Distinct().ToListAsync(cancellationToken);

        return dbOperationIdents.Select(this.securityRoleSource.GetSecurityRole).ToList();
    }
}
