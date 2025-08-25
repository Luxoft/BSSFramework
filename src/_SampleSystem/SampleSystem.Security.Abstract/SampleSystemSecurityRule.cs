using SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRule
{
    public static DomainSecurityRule.SecurityRuleHeader TestRestriction { get; } = new(nameof(TestRestriction));

    public static DomainSecurityRule.SecurityRuleHeader TestRoleGroup { get; } = new(nameof(TestRoleGroup));
}
