namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityRole
{

    public static SecurityRole TestRole3 { get; } = new SecurityRole(
        Guid.NewGuid(),
        nameof(TestRole3));

    public static SecurityRole TestRole2 { get; } = new SecurityRole(
        Guid.NewGuid(),
        nameof(TestRole2))
        {
            Children = [TestRole3]
        };

    public static SecurityRole TestRole { get; } = new SecurityRole(
        Guid.NewGuid(),
        nameof(TestRole))
        {
            Children = [TestRole2],
            Operations = [ExampleSecurityOperation.EmployeeView, ExampleSecurityOperation.EmployeeEdit]
        };

    public static SecurityRole SystemIntegration { get; } = new SecurityRole(
        Guid.NewGuid(),
        nameof(SystemIntegration));

    public static SecurityRole Administrator { get; } =
        SecurityRole.CreateAdministrator(
            Guid.NewGuid(),
            new[] { typeof(ExampleSecurityRole) });
}
