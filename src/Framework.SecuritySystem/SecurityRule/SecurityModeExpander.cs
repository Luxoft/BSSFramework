#nullable enable
using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityModeExpander
{
    private readonly AdministratorRoleInfo administratorRoleInfo;

    private readonly SystemIntegrationRoleInfo? systemIntegrationRoleInfo;

    private readonly IReadOnlyDictionary<Type, SecurityRule> viewDict;

    private readonly IReadOnlyDictionary<Type, SecurityRule> editDict;

    public SecurityModeExpander(
        IEnumerable<DomainObjectSecurityModeInfo> infos,
        AdministratorRoleInfo administratorRoleInfo,
        SystemIntegrationRoleInfo? systemIntegrationRoleInfo = null)
    {
        this.administratorRoleInfo = administratorRoleInfo;
        this.systemIntegrationRoleInfo = systemIntegrationRoleInfo;
        var cached = infos.ToList();

        this.viewDict = GetDict(cached, info => info.ViewRule);
        this.editDict = GetDict(cached, info => info.EditRule);
    }

    public SecurityRule? TryExpand<TDomainObject>(SecurityRule securityRule)
    {
        if (securityRule == SecurityRule.View)
        {
            return this.viewDict.GetValueOrDefault(typeof(TDomainObject));
        }
        else if (securityRule == SecurityRule.Edit)
        {
            return this.editDict.GetValueOrDefault(typeof(TDomainObject));
        }
        else if (securityRule == SpecialRoleSecurityRule.Administrator)
        {
            return this.administratorRoleInfo.AdministratorRole;
        }
        else if (securityRule == SpecialRoleSecurityRule.SystemIntegration)
        {
            return this.systemIntegrationRoleInfo?.SystemIntegrationRole ?? throw new ArgumentOutOfRangeException(nameof(securityRule), $"{nameof(SpecialRoleSecurityRule.SystemIntegration)}Role not defined");
        }

        return null;
    }

    private static Dictionary<Type, SecurityRule> GetDict(IEnumerable<DomainObjectSecurityModeInfo> infos, Func<DomainObjectSecurityModeInfo, SecurityRule?> selector)
    {
        var request = from info in infos

                      let securityRule = selector(info)

                      where securityRule != null

                      select info.DomainType.ToKeyValuePair(securityRule);

        return request.ToDictionary();
    }
}
