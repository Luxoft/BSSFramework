namespace Framework.SecuritySystem.Services;

public interface ISecurityRuleOptimizer
{
    SecurityRule.DomainSecurityRule Optimize(SecurityRule.DomainSecurityRule securityRule);
}
