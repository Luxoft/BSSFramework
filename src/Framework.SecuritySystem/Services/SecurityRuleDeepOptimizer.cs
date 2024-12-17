using Framework.Core;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleDeepOptimizer : ISecurityRuleDeepOptimizer
{
    private readonly ISecurityRuleExpander expander;

    private readonly ISecurityRuleBasicOptimizer basicOptimizer;

    private readonly IDictionaryCache<(DomainSecurityRule, SecurityRuleExpandSettings?), DomainSecurityRule> cache;

    public SecurityRuleDeepOptimizer(
        ISecurityRuleExpander expander,
        ISecurityRuleBasicOptimizer basicOptimizer)
    {
        this.expander = expander;
        this.basicOptimizer = basicOptimizer;
        this.cache = new DictionaryCache<(DomainSecurityRule, SecurityRuleExpandSettings?), DomainSecurityRule>(pair => this.Visit(pair.Item1, pair.Item2)).WithLock();
    }

    protected virtual DomainSecurityRule Visit(DomainSecurityRule baseSecurityRule, SecurityRuleExpandSettings? settings)
    {
        var visitedRule = this.basicOptimizer.Optimize(this.expander.FullDomainExpand(baseSecurityRule, settings));

        return visitedRule == baseSecurityRule ? visitedRule : this.Visit(visitedRule, settings);
    }

    DomainSecurityRule ISecurityRuleDeepOptimizer.Optimize(DomainSecurityRule securityRule, SecurityRuleExpandSettings? settings) =>
        this.cache[(securityRule, settings)];
}
