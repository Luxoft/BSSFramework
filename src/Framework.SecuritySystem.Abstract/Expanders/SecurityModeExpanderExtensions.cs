namespace Framework.SecuritySystem.Expanders;

public static class SecurityModeExpanderExtensions
{
    public static DomainSecurityRule Expand(this ISecurityModeExpander expander, DomainSecurityRule.DomainModeSecurityRule securityRule)
    {
        return expander.TryExpand(securityRule)
               ?? throw new ArgumentOutOfRangeException(
                   nameof(securityRule),
                   $"{nameof(SecurityRule)} with mode '{securityRule}' not found for type '{securityRule.DomainType.Name}'");
    }
}
