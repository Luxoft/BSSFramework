using Framework.SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRole
{
    public static SecurityRole SecretariatNotification { get; } = new(
        new Guid("8fd79f66-218a-47bc-9649-a07500fa6d11"),
        nameof(SecretariatNotification));

    public static SecurityRole SeManager { get; } = new(
        new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"),
        "SE Manager")
        {
            Operations = [SampleSystemSecurityOperation.EmployeeView]
        };

    public static SecurityRole TestRole1 { get; } = new(

        new Guid("{597AAB2A-76F7-42CF-B606-3D4550062596}"),
        nameof(TestRole1))
        {
            Operations = [SampleSystemSecurityOperation.EmployeeView]
        };

    public static SecurityRole TestRole2 { get; } = new(

        new Guid("{AD5EC94F-CC3D-451B-9051-B83059707E11}"),
        nameof(TestRole2))
        {
            Operations = [SampleSystemSecurityOperation.EmployeePositionView]
        };

    public static SecurityRole TestRole3 { get; } = new(

        new Guid("{B1B30E65-36BF-4ED1-9BD1-E614BA349507}"),
        nameof(TestRole3))
        {
            Operations = [SampleSystemSecurityOperation.EmployeeEdit]
        };

    public static SecurityRole SystemIntegration { get; } = new(
        new Guid("df74d544-5945-4380-944e-a3a9001252be"),
        nameof(SystemIntegration));

    public static SecurityRole Administrator { get; } = SecurityRole.CreateAdministrator(
        new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e"),
        [typeof(SampleSystemSecurityRole)],
        [typeof(SampleSystemSecurityOperation)]);
}
