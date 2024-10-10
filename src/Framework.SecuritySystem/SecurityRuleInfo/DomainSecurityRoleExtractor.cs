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

                new ScanVisitor(expander, usedRoles).Visit(securityRule);

                return usedRoles;
            }).WithLock();

    public IEnumerable<SecurityRole> Extract(DomainSecurityRule securityRule) => this.cache[securityRule];

    private class ScanVisitor(ISecurityRuleExpander expander, ISet<SecurityRole> usedRoles) : SecurityRuleVisitor
    {
        protected override DomainSecurityRule Visit(DomainSecurityRule.DomainModeSecurityRule securityRule)
        {
            return this.Visit(expander.Expand(securityRule));
        }

        protected override DomainSecurityRule Visit(DomainSecurityRule.RoleBaseSecurityRule baseSecurityRule)
        {
            var expandedRule = expander.FullExpand(baseSecurityRule);

            usedRoles.UnionWith(expandedRule.SecurityRoles);

            return baseSecurityRule;
        }
    }
}
