namespace Framework.SecuritySystem.Services;

public interface ISecurityRuleImplementationResolver
{
    DomainSecurityRule Resolve(DomainSecurityRule.SecurityRuleHeader securityRuleHeader);
}
