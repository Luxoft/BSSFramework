#nullable enable

namespace Framework.SecuritySystem;

public class SecurityModeExpander
{
    private readonly IReadOnlyDictionary<Type, SecurityRule.DomainObjectSecurityRule> viewDict;

    private readonly IReadOnlyDictionary<Type, SecurityRule.DomainObjectSecurityRule> editDict;

    private readonly IReadOnlyDictionary<Type, SecurityRule.DomainObjectSecurityRule> removeDict;

    public SecurityModeExpander(
        IEnumerable<DomainObjectSecurityModeInfo> infos)
    {
        var cached = infos.ToList();

        this.viewDict = GetDict(cached, info => info.ViewRule);
        this.editDict = GetDict(cached, info => info.EditRule);
        this.removeDict = GetDict(cached, info => info.RemoveRule);
    }

    public SecurityRule.DomainObjectSecurityRule? TryExpand<TDomainObject>(SecurityRule.SpecialSecurityRule securityRule)
    {
        if (securityRule == SecurityRule.View)
        {
            return this.viewDict.GetValueOrDefault(typeof(TDomainObject));
        }
        else if (securityRule == SecurityRule.Edit)
        {
            return this.editDict.GetValueOrDefault(typeof(TDomainObject));
        }
        else if (securityRule == SecurityRule.Remove)
        {
            return this.removeDict.GetValueOrDefault(typeof(TDomainObject))
                   ?? this.TryExpand<TDomainObject>(SecurityRule.Edit);
        }

        return null;
    }

    private static Dictionary<Type, SecurityRule.DomainObjectSecurityRule> GetDict(
        IEnumerable<DomainObjectSecurityModeInfo> infos,
        Func<DomainObjectSecurityModeInfo, SecurityRule.DomainObjectSecurityRule?> selector)
    {
        var request = from info in infos

                      let securityRule = selector(info)

                      where securityRule != null

                      select (info.DomainType, securityRule);

        return request.ToDictionary();
    }
}
