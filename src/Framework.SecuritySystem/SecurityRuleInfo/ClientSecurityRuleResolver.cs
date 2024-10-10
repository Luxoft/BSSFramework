using Framework.Core;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class ClientSecurityRuleResolver(
    IDomainSecurityRoleExtractor domainSecurityRoleExtractor,
    IClientSecurityRuleInfoSource clientSecurityRuleInfoSource) : IClientSecurityRuleResolver
{
    private readonly IDictionaryCache<SecurityRole, List<ClientSecurityRuleHeader>> cache =
        new DictionaryCache<SecurityRole, List<ClientSecurityRuleHeader>>(
            securityRole =>
            {
                var request = from clientSecurityRuleInfo in clientSecurityRuleInfoSource.GetInfos()

                              let roles = domainSecurityRoleExtractor.Extract(clientSecurityRuleInfo.Implementation)

                              where roles.Contains(securityRole)

                              select clientSecurityRuleInfo.Header;

                return request.ToList();
            }).WithLock();

    public IEnumerable<ClientSecurityRuleHeader> Resolve(SecurityRole securityRole) => this.cache[securityRole];
}
