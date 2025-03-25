using Automation.Settings;
using Automation.Utils.DatabaseUtils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Automation;

public class AssemblyInitializeAndCleanup(
    Func<IServiceProvider> getServiceProviderAction,
    Action<IServiceProvider> releaseServiceProviderAction)
    : AssemblyInitializeAndCleanupBase
{
    public async Task EnvironmentInitializeAsync()
    {
        var serviceProvider = getServiceProviderAction.Invoke();

        try
        {
            await this.InitializeAsync(serviceProvider);
        }
        finally
        {
            releaseServiceProviderAction(serviceProvider);
        }
    }

    public async Task EnvironmentCleanupAsync()
    {
        var serviceProvider = getServiceProviderAction.Invoke();
        try
        {
            await this.CleanupAsync(serviceProvider);
        }
        finally
        {
            releaseServiceProviderAction(serviceProvider);
        }
    }

    protected async Task CleanupAsync(IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetRequiredService<IOptions<AutomationFrameworkSettings>>();
        var databaseGenerator = serviceProvider.GetRequiredService<ITestDatabaseGenerator>();

        await this.CleanupAsync(settings.Value, databaseGenerator);
    }

    protected async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var settings = serviceProvider.GetRequiredService<IOptions<AutomationFrameworkSettings>>();
        var databaseGenerator = serviceProvider.GetRequiredService<ITestDatabaseGenerator>();

        await this.InitializeAsync(settings.Value, databaseGenerator);
    }
}
