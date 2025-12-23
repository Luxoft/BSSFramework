using SecuritySystem;

using SampleSystem.Domain;

using SecuritySystem.DependencyInjection;

using SecuritySystem.VirtualPermission.DependencyInjection;

namespace SampleSystem.Security;

public static class SampleSystemSecuritySystemExtensions
{
    public static ISecuritySystemSettings AddSecurityContexts(this ISecuritySystemSettings settings)
    {
        return settings.AddSecurityContext<BusinessUnit>(
            new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"),
            scb => scb.SetDisplayFunc(displayFunc: bu => bu.Name));
    }

    public static ISecuritySystemSettings AddSecurityRoles(this ISecuritySystemSettings settings)
    {
        return settings
               .AddSecurityRole(
                   SampleSystemSecurityRole.SeManager,
                   new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a")))

               .AddSecurityRole(
                   SecurityRole.Administrator,
                   new SecurityRoleInfo(new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e")));
    }

    public static ISecuritySystemSettings AddVirtualPermissions(this ISecuritySystemSettings settings)
    {
        return settings.AddVirtualPermission<Employee, BusinessUnitEmployeeRole>(
            link => link.Employee,
            vpb => vpb.ForRole(
                SampleSystemSecurityRole.SeManager,
                v => v.AddRestriction(link => link.BusinessUnit)
                      .AddFilter(link => link.Role == BusinessUnitEmployeeRoleType.Manager)));
    }
}
