﻿namespace Framework.SecuritySystem.DiTests;

public static class ExampleSecurityOperation
{
    public static SecurityOperation EmployeeView { get; } = new SecurityOperation(nameof(EmployeeView));

    public static SecurityOperation EmployeeEdit { get; } = new SecurityOperation(nameof(EmployeeEdit));
}

public static class ExampleSecurityRole
{
    public static SecurityRole TestRole { get; } = new SecurityRole(
        Guid.NewGuid(),
        nameof(TestRole),
        ExampleSecurityOperation.EmployeeView,
        ExampleSecurityOperation.EmployeeEdit);

    public static SecurityRole Administrator { get; } =
        SecurityRole.CreateAdministrator(
            Guid.NewGuid(),
            new[] { typeof(ExampleSecurityRole) });
}
