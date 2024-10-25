using Framework.Core;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class ClientSecurityRuleResolver(
    IDomainSecurityRoleExtractor domainSecurityRoleExtractor,
    IClientSecurityRuleInfoSource clientSecurityRuleInfoSource) : IClientSecurityRuleResolver
{
    private readonly IDictionaryCache<SecurityRole, List<DomainSecurityRule.ClientSecurityRule>> cache =
        new DictionaryCache<SecurityRole, List<DomainSecurityRule.ClientSecurityRule>>(
            securityRole =>
            {
                var request = from clientSecurityRuleInfo in clientSecurityRuleInfoSource.GetInfos()

                              let roles = domainSecurityRoleExtractor.ExtractSecurityRoles(clientSecurityRuleInfo.Implementation)

                              where roles.Contains(securityRole)

                              select clientSecurityRuleInfo.Rule;

                return request.ToList();
            }).WithLock();

    public IEnumerable<DomainSecurityRule.ClientSecurityRule> Resolve(SecurityRole securityRole) => this.cache[securityRole];
}
