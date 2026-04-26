using SampleSystem.WebApiCore.Domain;
using SampleSystem.WebApiCore.Security;

using Anch.SecuritySystem;
using Anch.SecuritySystem.DependencyInjection;

namespace SampleSystem.WebApiCore.DependencyInjection;

public static class SampleSystemDomainSecurityServiceExtensions
{
    public static ISecuritySystemSetup AddDomainSecurityServices(this ISecuritySystemSetup settings) =>
        settings.AddDomainSecurity(
                    SampleSystemSecurityRole.SeManager,
                    SecurityPath<BusinessUnit>.Create(bu => bu))

                .AddDomainSecurity(
                    SampleSystemSecurityRole.SeManager.Or(DomainSecurityRule.CurrentUser),
                    SecurityPath<Employee>.Create(
                        employee => employee.CoreBusinessUnit,
                        true));
}
