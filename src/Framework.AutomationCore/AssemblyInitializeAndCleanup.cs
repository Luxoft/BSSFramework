using Automation.Interfaces;
using Automation.Settings;
using Automation.Utils.DatabaseUtils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Automation;

public class AssemblyInitializeAndCleanup : AssemblyInitializeAndCleanupBase, IAssemblyInitializeAndCleanup
{
    private readonly Action<IServiceProvider> releaseServiceProviderAction;

    private readonly Func<IServiceProvider> getServiceProviderAction;

    public AssemblyInitializeAndCleanup(
            Func<IServiceProvider> getServiceProviderAction,
            Action<IServiceProvider> releaseServiceProviderAction)
    {
        this.getServiceProviderAction = getServiceProviderAction;
        this.releaseServiceProviderAction = releaseServiceProviderAction;
    }

    public async Task EnvironmentInitializeAsync()
    {
        var serviceProvider = this.getServiceProviderAction.Invoke();

        try
        {
            await this.InitializeAsync(serviceProvider);
        }
        finally
        {
            this.releaseServiceProviderAction(serviceProvider);
        }
    }

    public async Task EnvironmentCleanupAsync()
    {
        var serviceProvider = this.getServiceProviderAction.Invoke();
        try
        {
            await this.CleanupAsync(serviceProvider);
        }
        finally
        {
            this.releaseServiceProviderAction(serviceProvider);
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
