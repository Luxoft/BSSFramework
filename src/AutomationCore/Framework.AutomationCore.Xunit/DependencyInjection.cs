using Bss.Testing.Xunit.Interfaces;

using Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Xunit.ServiceEnvironment;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.Xunit;

public static class DependencyInjection
{
    public static IServiceCollection ApplyXunitIntegrationTestServices(this IServiceCollection services, Action<AutomationFrameworkSettings>? setup = null)
    {
        return services
               .AddSingleton<ITestInitializeAndCleanup, DiTestInitializeAndCleanup>()
               .ApplyIntegrationTestServices(setup);
    }
}
