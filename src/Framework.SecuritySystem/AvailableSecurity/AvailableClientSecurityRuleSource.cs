using Framework.SecuritySystem.SecurityRuleInfo;

namespace Framework.SecuritySystem.AvailableSecurity;

public class AvailableClientSecurityRuleSource(
    IAvailableSecurityRoleSource availableSecurityRoleSource,
    IClientSecurityRuleResolver clientSecurityRuleResolver) : IAvailableClientSecurityRuleSource
{
    public async Task<List<ClientSecurityRuleHeader>> GetAvailableSecurityRules(CancellationToken cancellationToken = default)
    {
        var roles = await availableSecurityRoleSource.GetAvailableSecurityRoles(true, cancellationToken);

        return roles.SelectMany(clientSecurityRuleResolver.Resolve)
                    .Distinct()
                    .ToList();
    }
}
