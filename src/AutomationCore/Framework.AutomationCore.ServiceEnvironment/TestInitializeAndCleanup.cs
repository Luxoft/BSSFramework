using Framework.AutomationCore.Enums;
using Framework.AutomationCore.ServiceEnvironment.Services;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Utils.DatabaseUtils;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;

using SecuritySystem.Testing;

namespace Framework.AutomationCore;

public class TestInitializeAndCleanup(
    IOptions<AutomationFrameworkSettings> settings,
    IDatabaseContext databaseContext,
    IntegrationTestTimeProvider timeProvider,
    RootImpersonateServiceState rootImpersonateServiceState)
{
    private readonly AutomationFrameworkSettings settings = settings.Value;

    public virtual async Task InitializeAsync()
    {
        this.ReattachDatabase();
        this.ResetServices();
    }

    public virtual async Task CleanupAsync()
    {
        this.DropDatabaseAfterTest();
        this.CleanupTestEnvironment();
    }

    protected virtual void ResetServices()
    {
        timeProvider.Reset();
        rootImpersonateServiceState.Reset();
    }

    protected virtual void DropDatabaseAfterTest()
    {
        if (this.settings.UseLocalDb || this.settings.TestRunMode == TestRunMode.DefaultRunModeOnEmptyDatabase || this.settings.TestsParallelize)
        {
            AssemblyInitializeAndCleanupBase.RunAction("Drop Database", databaseContext.Drop);
        }
    }

    protected virtual void ReattachDatabase()
    {
        switch (this.settings.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
            case TestRunMode.RestoreDatabaseUsingAttach:
                AssemblyInitializeAndCleanupBase.RunAction("Drop Database", databaseContext.Drop);
                AssemblyInitializeAndCleanupBase.RunAction("Restore Databases", databaseContext.AttachDatabase);
                break;
        }
    }

    protected virtual void CleanupTestEnvironment()
    {
    }
}
