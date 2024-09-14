namespace Framework.SecuritySystem;

public interface ISecuritySystemFactory
{
    ISecuritySystem Create(SecurityRuleCredential securityRuleCredential);
}
