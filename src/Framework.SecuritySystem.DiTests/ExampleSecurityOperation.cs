namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityOperation
{
    public static DisabledSecurityOperation Disabled { get; } = SecurityOperation.Disabled;

    public static SecurityOperation<Guid> EmployeeView { get; } = new SecurityOperation<Guid>(nameof(EmployeeView), Guid.NewGuid());

    public static SecurityOperation<Guid> EmployeeEdit { get; } = new SecurityOperation<Guid>(nameof(EmployeeEdit), Guid.NewGuid());
}
