using Framework.DomainDriven.Lock;
using Framework.DomainDriven.ServiceModel.IAD;

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
                           .AddSecurityOperationType(typeof(SampleSystemSecurityOperation))
                           .AddNamedLockType(typeof(SampleSystemNamedLock));
                   })

               .RegisterGeneralBssFramework()
               .RegisterGeneralDatabaseSettings(configuration)
               .RegisterGeneralApplicationServices(configuration)
               .RegisterGeneralSecurityServices();
    }
}
