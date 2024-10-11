using Framework.Core;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class DomainSecurityRoleExtractor(ISecurityRuleExpander expander) : IDomainSecurityRoleExtractor
{
    private readonly IDictionaryCache<DomainSecurityRule, IReadOnlySet<SecurityRole>> cache =
        new DictionaryCache<DomainSecurityRule, IReadOnlySet<SecurityRole>>(
            securityRule =>
            {
                var usedRoles = new HashSet<SecurityRole>();

                new ScanVisitor(usedRoles).Visit(expander.FullDomainExpand(securityRule));

                return usedRoles;
            }).WithLock();

    public IEnumerable<SecurityRole> Extract(DomainSecurityRule securityRule) => this.cache[securityRule];

    private class ScanVisitor(ISet<SecurityRole> usedRoles) : SecurityRuleVisitor
    {
        protected override DomainSecurityRule Visit(DomainSecurityRule.ExpandedRolesSecurityRule securityRule)
        {
            usedRoles.UnionWith(securityRule.SecurityRoles);

            return securityRule;
        }
    }
}
