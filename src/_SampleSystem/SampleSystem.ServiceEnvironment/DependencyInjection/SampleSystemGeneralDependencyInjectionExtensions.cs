using CommonFramework;

using Framework.Authorization.Environment;
using Framework.Infrastructure.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.EventMetadata;
using SampleSystem.Security;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection AddGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration, Action<IBssFrameworkBuilder> setupAction)
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
                                       .AddUserSource<Employee>(usb => usb.SetFilter(employee => employee.Active))
                                       .AddVirtualPermissions()
                                       .SetSecurityAdministratorRule(SampleSystemSecurityRole.PermissionAdministrator)

                                       .AddAuthorizationSystem()
                                       .AddConfigurationSecurity())

                           .AddNamedLocks()

                           .SetDomainObjectEventMetadata<SampleSystemDomainObjectEventMetadata>()

                           .AddListeners()

                           //.AddQueryVisitors()

                           // Legacy

                           .AddConfigurationSystemConstants()
                           .AddConfigurationTargetSystems()

                           .AddSubscriptionManagers()
                           .AddLegacyGenericServices()
                           .AddContextEvaluators()
                           .AddBLLSystem()

                           .AddSupportLegacyServices()
                           .Pipe(setupAction);
                   })

               .AddGeneralApplicationServices(configuration);
    }
}
