using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Security;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection RegisterGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        return services
               .RegisterGeneralBssFramework()
               .RegisterGeneralDatabaseSettings(configuration)
               .RegisterGeneralApplicationServices(configuration)
               .RegisterGeneralSecurityServices();
    }
}
