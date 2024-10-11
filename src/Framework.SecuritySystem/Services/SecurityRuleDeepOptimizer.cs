using Framework.Core;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleDeepOptimizer : ISecurityRuleDeepOptimizer
{
    private readonly ISecurityRuleExpander expander;

    private readonly ISecurityRuleBasicOptimizer basicOptimizer;

    private readonly IDictionaryCache<DomainSecurityRule, DomainSecurityRule> cache;

    public SecurityRuleDeepOptimizer(
        ISecurityRuleExpander expander,
        ISecurityRuleBasicOptimizer basicOptimizer)
    {
        this.expander = expander;
        this.basicOptimizer = basicOptimizer;
        this.cache = new DictionaryCache<DomainSecurityRule, DomainSecurityRule>(this.Visit).WithLock();
    }

    protected virtual DomainSecurityRule Visit(DomainSecurityRule baseSecurityRule)
    {
        var visitedRule = this.basicOptimizer.Optimize(this.expander.FullDomainExpand(baseSecurityRule));

        return visitedRule == baseSecurityRule ? visitedRule : this.Visit(visitedRule);
    }

    DomainSecurityRule ISecurityRuleDeepOptimizer.Optimize(DomainSecurityRule securityRule) =>
        this.cache[securityRule];
}
