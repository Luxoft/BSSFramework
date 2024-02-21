using Automation.Interfaces;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;

using Microsoft.Extensions.DependencyInjection;

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

    public void EnvironmentInitialize()
    {
        var serviceProvider = this.getServiceProviderAction.Invoke();

        try
        {
            this.Initialize(serviceProvider);
        }
        finally
        {
            this.releaseServiceProviderAction(serviceProvider);
        }
    }

    public void EnvironmentCleanup()
    {
        var serviceProvider = this.getServiceProviderAction.Invoke();
        try
        {
            this.Cleanup(serviceProvider);
        }
        finally
        {
            this.releaseServiceProviderAction(serviceProvider);
        }
    }

    protected void Cleanup(IServiceProvider serviceProvider)
    {
        var configUtil = serviceProvider.GetRequiredService<ConfigUtil>();
        var databaseGenerator = serviceProvider.GetRequiredService<ITestDatabaseGenerator>();

        this.Cleanup(configUtil, databaseGenerator);
    }

    protected void Initialize(IServiceProvider serviceProvider)
    {
        var configUtil = serviceProvider.GetRequiredService<ConfigUtil>();
        var databaseGenerator = serviceProvider.GetRequiredService<ITestDatabaseGenerator>();

        // if (!configUtil.UseLocalDb)
        // {
        //     RunAction("Check Server Name in allowed list", databaseGenerator.CheckServerAllowed);
        // }
        this.Initialize(configUtil, databaseGenerator);
    }
}
