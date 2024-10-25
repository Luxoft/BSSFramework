using Framework.Core;
using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem.SecurityRuleInfo;

public class DomainSecurityRoleExtractor : IDomainSecurityRoleExtractor
{
    private readonly IDictionaryCache<DomainSecurityRule, DomainSecurityRule.RoleBaseSecurityRule> rulesCache;

    private readonly IDictionaryCache<DomainSecurityRule, IReadOnlySet<SecurityRole>> rolesCache;

    public DomainSecurityRoleExtractor(ISecurityRuleExpander expander)
    {
        this.rulesCache =
            new DictionaryCache<DomainSecurityRule, DomainSecurityRule.RoleBaseSecurityRule>(
                securityRule =>
                {
                    var usedRules = new HashSet<DomainSecurityRule.ExpandedRolesSecurityRule>();

                    new ScanVisitor(usedRules).Visit(expander.FullDomainExpand(securityRule));

                    return usedRules.ToArray();
                }).WithLock();

        this.rolesCache =
            new DictionaryCache<DomainSecurityRule, IReadOnlySet<SecurityRole>>(
                securityRule => expander.FullRoleExpand(this.rulesCache[securityRule]).SecurityRoles.ToHashSet()).WithLock();
    }

    public IEnumerable<SecurityRole> ExtractSecurityRoles(DomainSecurityRule securityRule) => this.rolesCache[securityRule];

    public DomainSecurityRule.RoleBaseSecurityRule ExtractSecurityRule(DomainSecurityRule securityRule) => this.rulesCache[securityRule];

    private class ScanVisitor(ISet<DomainSecurityRule.ExpandedRolesSecurityRule> usedRules) : SecurityRuleVisitor
    {
        protected override DomainSecurityRule Visit(DomainSecurityRule.ExpandedRolesSecurityRule securityRule)
        {
            usedRules.Add(securityRule);

            return securityRule;
        }
    }
}
