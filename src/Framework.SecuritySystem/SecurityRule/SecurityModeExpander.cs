#nullable enable

namespace Framework.SecuritySystem;

public class SecurityModeExpander
{
    private readonly IReadOnlyDictionary<Type, SecurityRule.DomainSecurityRule> viewDict;

    private readonly IReadOnlyDictionary<Type, SecurityRule.DomainSecurityRule> editDict;

    public SecurityModeExpander(
        IEnumerable<DomainObjectSecurityModeInfo> infos)
    {
        var cached = infos.ToList();

        this.viewDict = GetDict(cached, info => info.ViewRule);
        this.editDict = GetDict(cached, info => info.EditRule);
    }

    public SecurityRule.DomainSecurityRule? TryExpand<TDomainObject>(SecurityRule securityRule)
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

    private static Dictionary<Type, SecurityRule.DomainSecurityRule> GetDict(
        IEnumerable<DomainObjectSecurityModeInfo> infos,
        Func<DomainObjectSecurityModeInfo, SecurityRule.DomainSecurityRule?> selector)
    {
        var request = from info in infos

                      let securityRule = selector(info)

                      where securityRule != null

                      select (info.DomainType, securityRule);

        return request.ToDictionary();
    }
}
