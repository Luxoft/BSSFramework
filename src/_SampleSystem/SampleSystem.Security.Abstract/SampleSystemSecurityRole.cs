using Framework.SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRole
{
    public static SecurityRole SecretariatNotification { get; } = new(nameof(SecretariatNotification));

    public static SecurityRole SeManager { get; } = new("SE Manager");

    public static SecurityRole TestRole1 { get; } = new(nameof(TestRole1));

    public static SecurityRole TestRole2 { get; } = new(nameof(TestRole2));

    public static SecurityRole TestRole3 { get; } = new(nameof(TestRole3));

    public static SecurityRole SearchTestBusinessRole { get; } = new(nameof(SearchTestBusinessRole));

    public static SecurityRole RestrictionRole { get; } = new(nameof(RestrictionRole));

    public static SecurityRole WithRestrictionFilterRole { get; } = new(nameof(WithRestrictionFilterRole));

    public static SecurityRole RequiredRestrictionRole { get; } = new(nameof(RequiredRestrictionRole));

    public static SecurityRole TestVirtualRole { get; } = new(nameof(TestVirtualRole));

    public static SecurityRole TestVirtualRole2 { get; } = new(nameof(TestVirtualRole2));

    public static SecurityRole PermissionAdministrator { get; } = new(nameof(PermissionAdministrator));

    public static SecurityRole TestPerformance { get; } = new(nameof(TestPerformance));
}
