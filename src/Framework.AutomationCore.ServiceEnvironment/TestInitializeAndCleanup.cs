using Automation.Enums;
using Automation.ServiceEnvironment.Services;
using Automation.Settings;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;

using SecuritySystem.Testing;

namespace Automation.ServiceEnvironment;

public class TestInitializeAndCleanup(
    IOptions<AutomationFrameworkSettings> settings,
    IDatabaseContext databaseContext,
    IntegrationTestTimeProvider timeProvider,
    ITestingUserAuthenticationService userAuthenticationService)
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
        userAuthenticationService.CustomUserCredential = null;
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
