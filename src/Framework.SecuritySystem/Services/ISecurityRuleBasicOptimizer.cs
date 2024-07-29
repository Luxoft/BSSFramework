namespace Framework.SecuritySystem.Services;

public interface ISecurityRuleBasicOptimizer
{
    DomainSecurityRule Optimize(DomainSecurityRule securityRule);
}
