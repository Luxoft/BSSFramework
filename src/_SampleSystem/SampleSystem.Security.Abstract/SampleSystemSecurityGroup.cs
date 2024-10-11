using Framework.SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityGroup
{
    public static DomainSecurityRule.SecurityRuleHeader TestGroup { get; } = new(nameof(TestGroup));
}
