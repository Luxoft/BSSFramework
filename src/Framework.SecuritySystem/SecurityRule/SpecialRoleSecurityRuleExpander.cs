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
        if (securityRule == SpecialRoleSecurityRule.Administrator)
        {
            return this.administratorRoleInfo.AdministratorRole.ToSecurityRule();
        }
        else if (securityRule == SpecialRoleSecurityRule.SystemIntegration)
        {
            return this.systemIntegrationRoleInfo?.SystemIntegrationRole?.ToSecurityRule()
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
