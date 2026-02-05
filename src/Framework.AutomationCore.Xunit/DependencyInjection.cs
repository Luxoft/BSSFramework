using Automation.ServiceEnvironment;
using Automation.Settings;
using Automation.Xunit.ServiceEnvironment;

using Bss.Testing.Xunit.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.Xunit;

public static class DependencyInjection
{
    public static IServiceCollection ApplyXunitIntegrationTestServices(this IServiceCollection services, Action<AutomationFrameworkSettings>? setup = null)
    {
        return services
               .AddSingleton<ITestInitializeAndCleanup, DiTestInitializeAndCleanup>()
               .ApplyIntegrationTestServices(setup);
    }
}
