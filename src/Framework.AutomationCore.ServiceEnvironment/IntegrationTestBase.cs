using Automation.Enums;
using Automation.ServiceEnvironment.Services;
using Automation.Utils.DatabaseUtils;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public abstract class IntegrationTestBase : RootServiceProviderContainer
{
    private readonly IServiceProviderPool rootServiceProviderPool;

    protected IntegrationTestBase(IServiceProviderPool rootServiceProviderPool)
        : base(rootServiceProviderPool.Get())
    {
        this.rootServiceProviderPool = rootServiceProviderPool;
    }

    public virtual void Initialize()
    {
        this.ReattachDatabase();
        this.ClearNotifications();
        this.ClearIntegrationEvents();
        this.ResetServices();
    }

    public virtual void Cleanup()
    {
        try
        {
            this.DropDatabaseAfterTest();
            this.CleanupTestEnvironment();
        }
        finally
        {
            this.ReleaseServiceProvider();
        }
    }

    protected virtual void ResetServices()
    {
        this.RootServiceProvider.GetService<IIntegrationTestUserAuthenticationService>()?.Reset();
        this.RootServiceProvider.GetService<IntegrationTestTimeProvider>()?.Reset();
    }

    protected virtual void DropDatabaseAfterTest()
    {
        if (this.AutomationFrameworkSettings.UseLocalDb || this.AutomationFrameworkSettings.TestRunMode == TestRunMode.DefaultRunModeOnEmptyDatabase)
        {
            AssemblyInitializeAndCleanupBase.RunAction("Drop Database", this.DatabaseContext.Drop);
        }
    }

    protected virtual void ReleaseServiceProvider()
        => this.rootServiceProviderPool.Release(this.RootServiceProvider);

    protected virtual void ReattachDatabase()
    {
        switch (this.AutomationFrameworkSettings.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
            case TestRunMode.RestoreDatabaseUsingAttach:
                AssemblyInitializeAndCleanupBase.RunAction("Drop Database", this.DatabaseContext.Drop);
                AssemblyInitializeAndCleanupBase.RunAction("Restore Databases", this.DatabaseContext.AttachDatabase);
                break;
        }
    }

    protected virtual void CleanupTestEnvironment()
    {
    }

    /// <summary>
    /// Отчистка списка нотифицаций
    /// </summary>
    public virtual void ClearNotifications()
    {
    }

    /// <summary>
    /// Отчистка интеграционных евентов
    /// </summary>
    public virtual void ClearIntegrationEvents()
    {
    }
}
