namespace Framework.SecuritySystem.Services;

public interface ISecurityRuleOptimizer
{
    DomainSecurityRule Optimize(DomainSecurityRule securityRule);
}
