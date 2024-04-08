#nullable enable
using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityModeExpander : ISecurityRuleExpander
{
    private readonly IReadOnlyDictionary<Type, SecurityRule> viewDict;

    private readonly IReadOnlyDictionary<Type, SecurityRule> editDict;

    public SecurityModeExpander(IEnumerable<DomainObjectSecurityModeInfo> infos)
    {
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
