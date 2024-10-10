namespace Framework.SecuritySystem.Expanders;

public interface ISecurityModeExpander
{
    DomainSecurityRule? TryExpand(DomainSecurityRule.DomainModeSecurityRule securityRule);
}
