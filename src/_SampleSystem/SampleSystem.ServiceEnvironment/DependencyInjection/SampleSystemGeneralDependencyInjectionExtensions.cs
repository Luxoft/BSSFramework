using CommonFramework;

using Framework.Authorization.Environment;
using Framework.Core;
using Framework.DomainDriven.Setup;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Security;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection RegisterGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration, Action<IBssFrameworkSettings> setupAction)
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
                                       .SetClientDomainModeSecurityRuleSource<SampleSystemClientDomainModeSecurityRuleSource>()
                                       .AddClientSecurityRuleInfoSource(typeof(SampleSystemSecurityGroup))
                                       .SetUserSource<Employee>(employee => employee.Id, employee => employee.Login, employee => employee.Active)
                                       .AddVirtualPermissions()
                                       .SetSecurityAdministratorRule(SampleSystemSecurityRole.PermissionAdministrator))

                           .AddAuthorizationSystem()

                           .AddNamedLocks()

                           .SetDomainObjectEventMetadata<SampleSystemDomainObjectEventMetadata>()

                           .AddListeners()

                           .AddQueryVisitors()

                           // Legacy

                           .AddConfigurationTargetSystems()

                           .AddSubscriptionManagers()
                           .AddLegacyGenericServices()
                           .AddContextEvaluators()
                           .AddBLLSystem()

                           .RegisterSupportLegacyServices()
                           .Pipe(setupAction);
                   })

               .RegisterGeneralApplicationServices(configuration);
    }
}
