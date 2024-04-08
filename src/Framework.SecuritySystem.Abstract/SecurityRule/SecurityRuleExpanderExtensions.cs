#nullable enable
namespace Framework.SecuritySystem;

public static class SecurityRuleExpanderExtensions
{
    public static SecurityRule? TryExpand<TDomainObject>(this IEnumerable<ISecurityRuleExpander> securityRuleExpanders, SecurityRule securityRule)
    {
        return securityRuleExpanders.Select(ex => ex.TryExpand<TDomainObject>(securityRule)).FirstOrDefault(result => result != null);
    }
}
