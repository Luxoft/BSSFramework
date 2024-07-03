#nullable enable
using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityModeExpander
{
    private readonly IReadOnlyDictionary<Type, List<SecurityRule>> viewDict;

    private readonly IReadOnlyDictionary<Type, List<SecurityRule>> editDict;

    public SecurityModeExpander(
        IEnumerable<DomainObjectSecurityModeInfo> infos)
    {
        var cached = infos.ToList();

        this.viewDict = GetDict(cached, info => info.ViewRules);
        this.editDict = GetDict(cached, info => info.EditRules);
    }

    public IReadOnlyList<SecurityRule> TryExpand<TDomainObject>(SecurityRule securityRule)
    {
        if (securityRule == SecurityRule.View)
        {
            return this.viewDict.GetValueOrDefault(typeof(TDomainObject)) ?? [];
        }
        else if (securityRule == SecurityRule.Edit)
        {
            return this.editDict.GetValueOrDefault(typeof(TDomainObject)) ?? [];
        }

        return [];
    }

    private static Dictionary<Type, List<SecurityRule>> GetDict(
        IEnumerable<DomainObjectSecurityModeInfo> infos,
        Func<DomainObjectSecurityModeInfo, IEnumerable<SecurityRule>> selector)
    {
        var request = from info in infos

                      let securityRule = selector(info)

                      where securityRule != null

                      select info.DomainType.ToKeyValuePair(RegGroupRules(securityRule).ToList());

        return request.ToDictionary();
    }

    private static IEnumerable<SecurityRule> RegGroupRules(IEnumerable<SecurityRule> securityRules)
    {
        return securityRules.Distinct()
                            .GroupBy(g => g.GetType())
                            .SelectMany(g => RegGroupRules(g, g.Key));
    }

    private static IEnumerable<SecurityRule> RegGroupRules(IEnumerable<SecurityRule> securityRules, Type securityRuleType)
    {
        if (securityRuleType == typeof(SecurityRule.NonExpandedRolesSecurityRule))
        {
            return securityRules.CastStrong<SecurityRule, SecurityRule.NonExpandedRolesSecurityRule>()
                                .GroupBy(securityRule => Tuple.Create(securityRule.CustomExpandType))
                                .Select(
                                    customExpandGroup =>
                                        customExpandGroup.SelectMany(sr => sr.SecurityRoles)
                                                         .ToSecurityRule(customExpandGroup.Key.Item1));
        }
        else if (securityRuleType == typeof(SecurityRule.ExpandedRolesSecurityRule))
        {
            return securityRules.CastStrong<SecurityRule, SecurityRule.ExpandedRolesSecurityRule>()
                                .GroupBy(securityRule => Tuple.Create(securityRule.CustomExpandType))
                                .Select(
                                    customExpandGroup =>
                                        new SecurityRule.ExpandedRolesSecurityRule(
                                        new DeepEqualsCollection<SecurityRole>(
                                            customExpandGroup.SelectMany(sr => sr.SecurityRoles)))
                                        {
                                            CustomExpandType = customExpandGroup.Key.Item1
                                        });
        }
        else
        {
            return securityRules;
        }
    }
}
