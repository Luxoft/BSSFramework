using Framework.Core;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class DomainModeSecurityRuleResolver(
    IDomainSecurityRoleExtractor domainSecurityRoleExtractor,
    IEnumerable<DomainModeSecurityRuleInfo> domainModeSecurityRuleInfoList) : IDomainModeSecurityRuleResolver
{
    private readonly IDictionaryCache<SecurityRole, List<DomainSecurityRule.DomainModeSecurityRule>> cache =
        new DictionaryCache<SecurityRole, List<DomainSecurityRule.DomainModeSecurityRule>>(
            securityRole =>
            {
                var request = from domainModeSecurityRuleInfo in domainModeSecurityRuleInfoList

                              let roles = domainSecurityRoleExtractor.ExtractSecurityRoles(domainModeSecurityRuleInfo.Implementation)

                              where roles.Contains(securityRole)

                              select domainModeSecurityRuleInfo.SecurityRule;

                return request.ToList();
            }).WithLock();

    public IEnumerable<DomainSecurityRule.DomainModeSecurityRule> Resolve(SecurityRole securityRole) => this.cache[securityRole];
}
