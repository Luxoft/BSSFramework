using Framework.Authorization;
using Framework.Configuration;
using Framework.SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRole
{
    public static SecurityRole Administrator { get; } = SecurityRole.CreateAdministrator(
        new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e"),
        new[] { typeof(AuthorizationSecurityOperation), typeof(ConfigurationSecurityOperation), typeof(SampleSystemSecurityOperation) });

    public static SecurityRole SystemIntegration { get; } = new SecurityRole(
        new Guid("df74d544-5945-4380-944e-a3a9001252be"),
        nameof(SystemIntegration),
        ConfigurationSecurityOperation.ProcessModifications,
        ConfigurationSecurityOperation.QueueMonitoring);


    public static SecurityRole SecretariatNotification { get; } = new SecurityRole(
        new Guid("8fd79f66-218a-47bc-9649-a07500fa6d11"),
        nameof(SecretariatNotification));

    public static SecurityRole SeManager { get; } = new SecurityRole(
        new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"),
        "SE Manager",
        SampleSystemSecurityOperation.EmployeeView);

    public static SecurityRole TestRole1 { get; } = new SecurityRole(

        new Guid("{597AAB2A-76F7-42CF-B606-3D4550062596}"),
        nameof(TestRole1),
        SampleSystemSecurityOperation.EmployeeView);

    public static SecurityRole TestRole2 { get; } = new SecurityRole(

        new Guid("{AD5EC94F-CC3D-451B-9051-B83059707E11}"),
        nameof(TestRole2),
        SampleSystemSecurityOperation.EmployeePositionView);

}
