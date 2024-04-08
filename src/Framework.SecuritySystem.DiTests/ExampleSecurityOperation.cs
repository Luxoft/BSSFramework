namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityOperation
{
    public static SecurityOperation EmployeeView { get; } = new SecurityOperation(nameof(EmployeeView));

    public static SecurityOperation EmployeeEdit { get; } = new SecurityOperation(nameof(EmployeeEdit));
}
