using Framework.DomainDriven.Setup;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Security;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection RegisterGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        return services

               .AddBssFramework(
                   rootSettings =>
                   {
                       rootSettings.AddSecuritySystem(
                           securitySettings =>
                               securitySettings
                                   .AddSecurityContexts()
                                   .AddDomainSecurityServices()
                                   .AddSecurityRoles()
                                   .AddCustomSecurityOperations())

                           .AddNamedLockType(typeof(SampleSystemNamedLock))

                           .SetDomainObjectEventMetadata<SampleSystemDomainObjectEventMetadata>()

                           .SetPrincipalIdentitySource((Employee employee) => employee.Login)

                           .AddListeners()

                           // Legacy

                           .AddSubscriptionManagers()
                           .AddLegacyGenericServices()
                           .AddContextEvaluators()
                           .AddBLLSystem();
                   })

               .RegisterLegacyGeneralBssFramework()
               .RegisterGeneralDatabaseSettings(configuration)
               .RegisterGeneralApplicationServices(configuration);
    }
}
