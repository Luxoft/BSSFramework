using Framework.SecuritySystem;

using SampleSystem.Security.Services;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRule
{
    public static DomainSecurityRule.ConditionSecurityRule TestRestriction { get; } = new(typeof(TestRestrictionObjectConditionFactory<>));
}
