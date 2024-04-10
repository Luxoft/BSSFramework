using Framework.DomainDriven.ServiceModel.IAD;
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
                   settings =>
                   {
                       settings
                           .AddSecurityRoleTypeType(typeof(SampleSystemSecurityRole))
                           .AddNamedLockType(typeof(SampleSystemNamedLock))

                           .AddSecurityContext<BusinessUnit>(new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"))
                           .AddSecurityContext<Location>(new Guid("4641395B-9079-448E-9CB8-A083015235A3"))
                           .AddSecurityContext<ManagementUnit>(new Guid("77E78AEF-9512-46E0-A33D-AAE58DC7E18C"))
                           .AddSecurityContext<Employee>(new Guid("B3F2536E-27C4-4B91-AE0B-0EE2FFD4465F"), displayFunc: employee => employee.Login)

                           .SetAdministratorRole(SampleSystemSecurityRole.Administrator)
                           .SetSystemIntegrationRole(SampleSystemSecurityRole.SystemIntegration)

                           .AddDomainSecurityServices()

                           .SetDomainObjectEventMetadata<SampleSystemDomainObjectEventMetadata>()

                           .AddListener<SubscriptionDALListener>()

                           .AddListener<ExampleFaultDALListener>(true);
                   })

               .RegisterLegacyGeneralBssFramework()
               .RegisterGeneralDatabaseSettings(configuration)
               .RegisterGeneralApplicationServices(configuration);
    }
}
