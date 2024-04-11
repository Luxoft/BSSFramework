namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityRole
{
    public static SecurityRole TestRole { get; } = new SecurityRole(
        Guid.NewGuid(),
        nameof(TestRole),
        ExampleSecurityOperation.EmployeeView,
        ExampleSecurityOperation.EmployeeEdit);

    public static SecurityRole SystemIntegration { get; } = new SecurityRole(
        Guid.NewGuid(),
        nameof(SystemIntegration));

    public static SecurityRole Administrator { get; } =
        SecurityRole.CreateAdministrator(
            Guid.NewGuid(),
            new[] { typeof(ExampleSecurityRole) });
}
