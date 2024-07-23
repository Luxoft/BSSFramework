using Framework.SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRule
{
    public static DomainSecurityRule.SecurityRuleHeader TestRestriction { get; } = new(nameof(TestRestriction));
}
