using Framework.Authorization.Environment;
using Framework.DomainDriven.Setup;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using nuSpec.NHibernate;

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
                       rootSettings
                           .AddSecuritySystem(
                               securitySettings =>
                                   securitySettings
                                       .AddSecurityContexts()
                                       .AddDomainSecurityServices()
                                       .AddSecurityRoles()
                                       .AddSecurityRules()
                                       .AddCustomSecurityOperations()
                                       .SetUserSource<Employee>(employee => employee.Id, employee => employee.Login, employee => employee.Active))

                           .AddAuthorizationSystem()

                           .SetSecurityAdministratorRule(SampleSystemSecurityRole.PermissionAdministrator)

                           .AddNamedLockType(typeof(SampleSystemNamedLock))

                           .SetDomainObjectEventMetadata<SampleSystemDomainObjectEventMetadata>()

                           .AddListeners()

                           .SetSpecificationEvaluator<NhSpecificationEvaluator>()
                           .AddDatabaseSettings(configuration)
                           .AddDatabaseVisitors()

                           // Legacy

                           .AddLegacyDatabaseSettings()
                           .AddConfigurationTargetSystems()

                           .AddSubscriptionManagers()
                           .AddLegacyGenericServices()
                           .AddContextEvaluators()
                           .AddBLLSystem()

                           .RegisterSupportLegacyServices();
                   })

               .RegisterGeneralApplicationServices(configuration);
    }
}
