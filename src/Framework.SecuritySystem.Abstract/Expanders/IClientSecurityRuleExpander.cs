namespace Framework.SecuritySystem.Expanders;

public interface IClientSecurityRuleExpander
{
    DomainSecurityRule Expand(DomainSecurityRule.ClientSecurityRule securityRule);
}
