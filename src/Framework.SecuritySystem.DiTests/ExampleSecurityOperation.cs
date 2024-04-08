namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityOperation
{
    public static SecurityRule Disabled { get; } = SecurityRule.Disabled;

    public static SecurityRule EmployeeView { get; } = new SecurityRule(nameof(EmployeeView));

    public static SecurityRule EmployeeEdit { get; } = new SecurityRule(nameof(EmployeeEdit));
}
