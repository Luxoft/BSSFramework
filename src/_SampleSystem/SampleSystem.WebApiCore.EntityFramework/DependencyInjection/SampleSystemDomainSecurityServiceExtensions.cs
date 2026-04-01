using SampleSystem.WebApiCore.Domain;
using SampleSystem.WebApiCore.Security;

using SecuritySystem;
using SecuritySystem.DependencyInjection;

namespace SampleSystem.WebApiCore.DependencyInjection;

public static class SampleSystemDomainSecurityServiceExtensions
{
    public static ISecuritySystemBuilder AddDomainSecurityServices(this ISecuritySystemBuilder settings)
    {
        return settings.AddDomainSecurity(
                           SampleSystemSecurityRole.SeManager,
                           SecurityPath<BusinessUnit>.Create(bu => bu))

                       .AddDomainSecurity(
                           SampleSystemSecurityRole.SeManager.Or(DomainSecurityRule.CurrentUser),
                           SecurityPath<Employee>.Create(
                               employee => employee.CoreBusinessUnit,
                               true));
    }
}
