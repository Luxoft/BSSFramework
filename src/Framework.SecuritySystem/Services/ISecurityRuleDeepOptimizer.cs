namespace Framework.SecuritySystem.Services;

public interface ISecurityRuleDeepOptimizer
{
    DomainSecurityRule Optimize(DomainSecurityRule securityRule);
}
