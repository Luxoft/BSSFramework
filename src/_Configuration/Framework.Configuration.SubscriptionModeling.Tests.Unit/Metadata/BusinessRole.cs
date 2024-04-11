using Framework.SecuritySystem;

namespace Framework.Configuration.SubscriptionModeling.Tests.Unit.Metadata;

internal static class TestSecurityRole
{
    public static SecurityRole Administrator { get; } = SecurityRole.CreateAdministrator(
        Guid.Parse("{137C3ADD-64F4-40E9-BF33-D42159267DA6}"),
        new List<Type>());
}
