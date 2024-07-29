using Framework.Core;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleDeepOptimizer : ISecurityRuleDeepOptimizer
{
    private readonly ISecurityRuleExpander expander;

    private readonly ISecurityRuleBasicOptimizer basicOptimizer;

    private readonly ISecurityRuleImplementationResolver implementationResolver;

    private readonly IDictionaryCache<DomainSecurityRule, DomainSecurityRule> cache;

    public SecurityRuleDeepOptimizer(
        ISecurityRuleExpander expander,
        ISecurityRuleBasicOptimizer basicOptimizer,
        ISecurityRuleImplementationResolver implementationResolver)
    {
        this.expander = expander;
        this.basicOptimizer = basicOptimizer;
        this.implementationResolver = implementationResolver;
        this.cache = new DictionaryCache<DomainSecurityRule, DomainSecurityRule>(this.Visit).WithLock();
    }

    protected virtual DomainSecurityRule Visit(DomainSecurityRule baseSecurityRule)
    {
        var visitedRule = this.InternalVisit(baseSecurityRule);

        return visitedRule == baseSecurityRule ? visitedRule : this.Visit(visitedRule);
    }

    protected virtual DomainSecurityRule InternalVisit(DomainSecurityRule baseSecurityRule)
    {
        switch (baseSecurityRule)
        {
            case DomainSecurityRule.RoleBaseSecurityRule securityRule:
                return this.expander.FullExpand(securityRule);

            case DomainSecurityRule.SecurityRuleHeader securityRuleHeader:
                return this.implementationResolver.Resolve(securityRuleHeader);

            default:
                return this.basicOptimizer.Optimize(baseSecurityRule);
        }
    }

    DomainSecurityRule ISecurityRuleDeepOptimizer.Optimize(DomainSecurityRule securityRule) =>
        this.cache[securityRule];
}
