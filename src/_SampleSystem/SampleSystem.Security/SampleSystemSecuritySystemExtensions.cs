using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;

using SampleSystem.Domain;

namespace SampleSystem.Security;

public static class SampleSystemSecuritySystemExtensions
{
    public static ISecuritySystemSettings AddSecurityContexts(this ISecuritySystemSettings settings)
    {
        return settings.AddSecurityContext<BusinessUnit>(new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"))
                       .AddSecurityContext<Location>(new Guid("4641395B-9079-448E-9CB8-A083015235A3"))
                       .AddSecurityContext<ManagementUnit>(new Guid("77E78AEF-9512-46E0-A33D-AAE58DC7E18C"))
                       .AddSecurityContext<Employee>(new Guid("B3F2536E-27C4-4B91-AE0B-0EE2FFD4465F"), displayFunc: employee => employee.Login);
    }

    public static ISecuritySystemSettings AddSecurityRoles(this ISecuritySystemSettings settings)
    {
        return settings
               .AddSecurityRole(
                   SampleSystemSecurityRole.SecretariatNotification,
                   new SecurityRoleInfo(new Guid("8fd79f66-218a-47bc-9649-a07500fa6d11")))

               .AddSecurityRole(
                   SampleSystemSecurityRole.SeManager,
                   new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeeView]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.TestRole1,
                   new SecurityRoleInfo(new Guid("{597AAB2A-76F7-42CF-B606-3D4550062596}"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeeView]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.TestRole2,
                   new SecurityRoleInfo(new Guid("{AD5EC94F-CC3D-451B-9051-B83059707E11}"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeePositionView]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.TestRole3,
                   new SecurityRoleInfo(new Guid("{B1B30E65-36BF-4ED1-9BD1-E614BA349507}"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeeEdit]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.SearchTestBusinessRole,
                   new SecurityRoleInfo(new Guid("{05271C71-7E6B-430A-9EC7-F838845D0F33}")))

               .AddSecurityRole(
                   SampleSystemSecurityRole.RestrictionRole,
                   new SecurityRoleInfo(new Guid("{ACAA7B42-09AA-438A-B6EC-058506E0C103}"))
                   {
                       Restriction = SecurityPathRestriction.Create<BusinessUnit>()
                                                            .Add((TestRestrictionObject v) => v.RestrictionHandler)
                   })

               .AddSecurityRole(
                   SecurityRole.SystemIntegration,
                   new SecurityRoleInfo(new Guid("df74d544-5945-4380-944e-a3a9001252be")))

               .AddSecurityRole(
                   SecurityRole.Administrator,
                   new SecurityRoleInfo(new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e"))
                   {
                       Operations = typeof(SampleSystemSecurityOperation).GetStaticPropertyValueList<SecurityOperation>().ToList()
                   });
    }

    public static ISecuritySystemSettings AddCustomSecurityOperations(this ISecuritySystemSettings settings)
    {
        return settings.AddSecurityOperation(
                           SampleSystemSecurityOperation.BusinessUnitView,
                           new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All })

                       .AddSecurityOperation(
                           SampleSystemSecurityOperation.ManagementUnitView,
                           new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All });
    }
}
