using SecuritySystem;

using SampleSystem.Domain;

using SecuritySystem.DependencyInjection;

namespace SampleSystem.Security;

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
