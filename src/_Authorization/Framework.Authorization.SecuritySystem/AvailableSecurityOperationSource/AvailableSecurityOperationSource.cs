using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AvailableSecurityRoleSource : IAvailableSecurityRoleSource
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly ISecurityOperationParser parser;

    public AvailableSecurityRoleSource(IAvailablePermissionSource availablePermissionSource, ISecurityOperationParser parser)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.parser = parser;
    }

    public async Task<List<SecurityRole>> GetAvailableSecurityRole (CancellationToken cancellationToken)
    {
        var dbRequest = from permission in this.availablePermissionSource.GetAvailablePermissionsQueryable()

                        select permission.Role.Id;

        var dbOperationIdents = await dbRequest.Distinct().ToListAsync(cancellationToken);

        return dbOperationIdents.Select(this.parser.GetSecurityOperation).ToList<SecurityRole>();
    }
}
