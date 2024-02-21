using Framework.Authorization;
using Framework.Authorization.SecuritySystem;
using Framework.Configuration;
using Framework.SecuritySystem.Bss;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRole
{
    public static SecurityRole Administrator { get; } = SecurityRole.CreateAdministrator(
        new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e"),
        new[] { typeof(AuthorizationSecurityOperation), typeof(ConfigurationSecurityOperation), typeof(SampleSystemSecurityOperation) });

    public static SecurityRole SystemIntegration { get; } = new SecurityRole(
        new Guid("df74d544-5945-4380-944e-a3a9001252be"),
        nameof(SystemIntegration),
        BssSecurityOperation.SystemIntegration,
        ConfigurationSecurityOperation.ProcessModifications,
        ConfigurationSecurityOperation.QueueMonitoring);


    public static SecurityRole SecretariatNotification { get; } = new SecurityRole(
        new Guid("8fd79f66-218a-47bc-9649-a07500fa6d11"),
        nameof(SecretariatNotification));

    public static SecurityRole SeManager { get; } = new SecurityRole(
        new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"),
        "SE Manager",
        SampleSystemSecurityOperation.EmployeeView);

    public static SecurityRole TestChildRole { get; } = new SecurityRole(

        new Guid("{9F080934-5009-4253-B537-99A2211C3474}"),
        nameof(TestChildRole),
        SampleSystemSecurityOperation.EmployeeView,
        SampleSystemSecurityOperation.BusinessUnitView);

    public static SecurityRole TestRootRole { get; } = new SecurityRole(

        new Guid("{20C220CF-D5CE-4F0B-98C7-1211A45845D1}"),
        nameof(TestRootRole),
        SampleSystemSecurityOperation.EmployeeEdit,
        SampleSystemSecurityOperation.BusinessUnitView) { Children = new[] { TestChildRole } };
}
