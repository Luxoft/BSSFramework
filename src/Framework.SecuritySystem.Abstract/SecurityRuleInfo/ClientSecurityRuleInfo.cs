namespace Framework.SecuritySystem.SecurityRuleInfo;

public record ClientSecurityRuleInfo(DomainSecurityRule.ClientSecurityRule Rule, DomainSecurityRule Implementation)
{
    public ClientSecurityRuleInfo(string name, DomainSecurityRule implementation)
        : this(new DomainSecurityRule.ClientSecurityRule(name), implementation)
    {
    }
}
