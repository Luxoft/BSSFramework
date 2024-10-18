namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityRole
{
    public static SecurityRole TestRole { get; } = new(nameof(TestRole));

    public static SecurityRole TestRole2 { get; } = new(nameof(TestRole2));

    public static SecurityRole TestRole3 { get; } = new(nameof(TestRole3));

    public static SecurityRole TestRole4 { get; } = new(nameof(TestRole4));

    public static SecurityRole TestKeyedRole { get; } = new(nameof(TestKeyedRole));
}
