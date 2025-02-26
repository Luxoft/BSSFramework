using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services;

public interface ISecurityRuleDeepOptimizer
{
    DomainSecurityRule Optimize(DomainSecurityRule securityRule, SecurityRuleExpandSettings? settings = null);
}
