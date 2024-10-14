namespace Framework.SecuritySystem.Expanders;

public interface ISecurityRuleHeaderExpander
{
    DomainSecurityRule Expand(DomainSecurityRule.SecurityRuleHeader securityRuleHeader);
}
