using CommonFramework;

using Framework.Authorization.Environment;
using Framework.Infrastructure.DependencyInjection;
using Framework.Subscriptions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SampleSystem.Domain;
using SampleSystem.EventMetadata;
using SampleSystem.Security;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection AddGeneralDependencyInjection(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment hostEnvironment,
        Action<IBssFrameworkSetup> setupAction) =>
        services

            .AddBssFramework(rootSettings =>
            {
                rootSettings
                    .AddSecuritySystem(securitySettings =>
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

                    // Legacy

                    .AddSubscriptions<IBssFrameworkSetup, Employee, Framework.Authorization.Domain.Principal>(
                        e => e.Email,
                        [typeof(SampleSystem.Subscriptions.Metadata.Employee.Update.EmployeeUpdateSubscription).Assembly])

                    .AddSubscriptionManagers()
                    .AddLegacyGenericServices()
                    .AddContextEvaluators()

                    .AddLegacyDefaultGenericServices()
                    .AddConfigurationSystemConstants()
                    .AddConfigurationTargetSystems()

                    .AddBLLSystem()

                    .AddSupportLegacyServices()
                    .Pipe(setupAction);
            })

            .AddGeneralApplicationServices(configuration, hostEnvironment);
}
