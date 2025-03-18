using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;

namespace SampleSystem.Security;

public static class SampleSystemDomainSecurityServiceExtensions
{
    public static ISecuritySystemSettings AddDomainSecurityServices(this ISecuritySystemSettings settings)
    {
        return settings.AddDomainSecurityServices(
            rb => rb

                  .Add<BusinessUnit>(
                      SampleSystemSecurityRole.SeManager,
                      SecurityPath<BusinessUnit>.Create(bu => bu))

                  .Add<Employee>(
                      SampleSystemSecurityRole.SeManager.Or(DomainSecurityRule.CurrentUser),
                      SecurityPath<Employee>.Create(
                          employee => employee.CoreBusinessUnit,
                          true)));
    }
}
