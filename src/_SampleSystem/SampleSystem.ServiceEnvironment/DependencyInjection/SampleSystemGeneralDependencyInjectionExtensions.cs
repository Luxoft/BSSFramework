using Anch.Core;

using Framework.Authorization.Environment;
using Framework.Configuration.BLL.Notification;
using Framework.Infrastructure.DependencyInjection;
using Framework.Notification.DependencyInjection;
using Framework.Subscriptions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SampleSystem.Domain.Employee;
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
                                               .AddConfigurationSecurity()
                                               .AddPrincipalManagementListener<SamplePrincipalManagementListener>())

                    .AddNamedLocks()

                    .SetDomainObjectEventMetadata<SampleSystemDomainObjectEventMetadata>()

                    .AddDatabase(ds => ds.AddVisitorContainer<CalculatedProjectPropertyVisitorContainer>())

                    .AddListeners()

                    .AddNotification(configuration, ns => ns.SetSender<LocalDbNotificationMessageSender>())

                    // Legacy
                    .AddSubscriptions(
                        [typeof(SampleSystem.Subscriptions.Metadata.Employee.Update.EmployeeUpdateSubscription).Assembly],
                        (Employee e) => e.Email)

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
