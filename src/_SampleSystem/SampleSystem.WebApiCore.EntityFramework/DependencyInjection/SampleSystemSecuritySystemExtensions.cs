using SampleSystem.WebApiCore.Domain;
using SampleSystem.WebApiCore.Security;

using SecuritySystem;
using SecuritySystem.DependencyInjection;
using SecuritySystem.VirtualPermission.DependencyInjection;

namespace SampleSystem.WebApiCore.DependencyInjection;

public static class SampleSystemSecuritySystemExtensions
{
    public static ISecuritySystemBuilder AddSecurityContexts(this ISecuritySystemBuilder settings)
    {
        return settings.AddSecurityContext<BusinessUnit>(
            new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"),
            scb => scb.SetDisplayFunc(displayFunc: bu => bu.Name));
    }

    public static ISecuritySystemBuilder AddSecurityRoles(this ISecuritySystemBuilder settings)
    {
        return settings
               .AddSecurityRole(
                   SampleSystemSecurityRole.SeManager,
                   new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a")))

               .AddSecurityRole(
                   SecurityRole.Administrator,
                   new SecurityRoleInfo(new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e")));
    }

    public static ISecuritySystemBuilder AddVirtualPermissions(this ISecuritySystemBuilder settings)
    {
        return settings.AddVirtualPermission<Employee, BusinessUnitEmployeeRole>(
            link => link.Employee,
            vpb => vpb.AddRestriction(link => link.BusinessUnit)
                      .AddSecurityRole(
                          SampleSystemSecurityRole.SeManager,
                          v => v.AddFilter(link => link.Role == BusinessUnitEmployeeRoleType.Manager)));
    }
}
