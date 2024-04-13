#nullable enable
namespace Framework.SecuritySystem;

public class SpecialRoleSecurityRuleExpander
{
    private readonly AdministratorRoleInfo administratorRoleInfo;

    private readonly SystemIntegrationRoleInfo? systemIntegrationRoleInfo;

    public SpecialRoleSecurityRuleExpander(
        AdministratorRoleInfo administratorRoleInfo,
        SystemIntegrationRoleInfo? systemIntegrationRoleInfo = null)
    {
        this.administratorRoleInfo = administratorRoleInfo;
        this.systemIntegrationRoleInfo = systemIntegrationRoleInfo;
    }

    public SecurityRule.NonExpandedRolesSecurityRule Expand(SpecialRoleSecurityRule securityRule)
    {
        if (securityRule.Name == SpecialRoleSecurityRule.Administrator.Name)
        {
            return this.administratorRoleInfo.AdministratorRole.ToSecurityRule(securityRule.ExpandType);
        }
        else if (securityRule.Name == SpecialRoleSecurityRule.SystemIntegration.Name)
        {
            return this.systemIntegrationRoleInfo?.SystemIntegrationRole?.ToSecurityRule(securityRule.ExpandType)
                   ?? throw new ArgumentOutOfRangeException(
                       nameof(securityRule),
                       $"{nameof(SpecialRoleSecurityRule.SystemIntegration)}Role not defined");
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(securityRule));
        }
    }
}
