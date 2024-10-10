namespace Framework.SecuritySystem.SecurityRuleInfo;

public record ClientSecurityRuleInfo(ClientSecurityRuleHeader Header, DomainSecurityRule Implementation)
{
    public ClientSecurityRuleInfo(string name, DomainSecurityRule implementation)
        : this(new ClientSecurityRuleHeader(name), implementation)
    {
    }
}
