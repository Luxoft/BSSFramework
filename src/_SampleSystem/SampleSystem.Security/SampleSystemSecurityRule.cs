using Framework.SecuritySystem;

using SampleSystem.Security.Services;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRule
{
    public static SecurityRule.ConditionSecurityRule TestRestriction { get; } = new(typeof(TestRestrictionObjectConditionFactory<>));
}
