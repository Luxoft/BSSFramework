namespace Framework.SecuritySystem;

public class AvailableSecurityOperationSource(IAvailableSecurityRoleSource availableSecurityRoleSource)
    : IAvailableSecurityOperationSource
{
    public async Task<List<SecurityOperation>> GetAvailableSecurityOperations(CancellationToken cancellationToken = default)
    {
        var roles = await availableSecurityRoleSource.GetAvailableSecurityRoles(true, cancellationToken);

        return roles.SelectMany(sr => sr.Information.Operations)
                    .Distinct()
                    .ToList();
    }
}
