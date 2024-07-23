using Framework.Core;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleDeepOptimizer : ISecurityRuleDeepOptimizer
{
    private readonly ISecurityRuleExpander expander;

    private readonly ISecurityRuleBasicOptimizer basicOptimizer;

    private readonly IDictionaryCache<DomainSecurityRule, DomainSecurityRule> cache;

    public SecurityRuleDeepOptimizer(ISecurityRuleExpander expander, ISecurityRuleBasicOptimizer basicOptimizer)
    {
        this.expander = expander;
        this.basicOptimizer = basicOptimizer;
        this.cache = new DictionaryCache<DomainSecurityRule, DomainSecurityRule>(this.Visit).WithLock();
    }

    protected virtual DomainSecurityRule Visit(DomainSecurityRule baseSecurityRule)
    {
        switch (baseSecurityRule)
        {
            case DomainSecurityRule.RoleBaseSecurityRule roleBaseSecurityRule:

        }
    }

    protected virtual DomainSecurityRule InternalVisit(DomainSecurityRule baseSecurityRule)
    {
        switch (baseSecurityRule)
        {
            case DomainSecurityRule.RoleBaseSecurityRule securityRule:
                return this.expander.FullExpand(securityRule);
        }
    }

    DomainSecurityRule ISecurityRuleOptimizer.Optimize(DomainSecurityRule securityRule) =>
        this.cache[securityRule];
}
