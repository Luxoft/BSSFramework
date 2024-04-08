using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AvailableSecurityOperationSource : IAvailableSecurityOperationSource
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly ISecurityOperationParser<Guid> parser;

    public AvailableSecurityOperationSource(IAvailablePermissionSource availablePermissionSource, ISecurityOperationParser<Guid> parser)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.parser = parser;
    }

    public async Task<List<SecurityRule>> GetAvailableSecurityOperation (CancellationToken cancellationToken)
    {
        var dbRequest = (from permission in this.availablePermissionSource.GetAvailablePermissionsQueryable()

                        from operationLink in permission.Role.BusinessRoleOperationLinks

                        select operationLink.Operation.Id)
                        .Distinct();

        var dbOperationIdents = await dbRequest.ToListAsync(cancellationToken);

        return dbOperationIdents.Select(this.parser.GetSecurityOperation).ToList<SecurityRule>();
    }
}
