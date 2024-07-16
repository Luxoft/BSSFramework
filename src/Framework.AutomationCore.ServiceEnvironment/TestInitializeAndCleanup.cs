using Automation.Enums;
using Automation.Interfaces;
using Automation.ServiceEnvironment.Services;
using Automation.Settings;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;

namespace Automation.ServiceEnvironment;

public class TestInitializeAndCleanup : ITestInitializeAndCleanup
{
    private readonly AutomationFrameworkSettings settings;

    private readonly IDatabaseContext databaseContext;

    private readonly IntegrationTestTimeProvider timeProvider;

    private readonly IIntegrationTestUserAuthenticationService userAuthenticationService;

    public TestInitializeAndCleanup(
        IOptions<AutomationFrameworkSettings> settings,
        IDatabaseContext databaseContext,
        IntegrationTestTimeProvider timeProvider,
        IIntegrationTestUserAuthenticationService userAuthenticationService)
    {
        this.settings = settings.Value;
        this.databaseContext = databaseContext;
        this.timeProvider = timeProvider;
        this.userAuthenticationService = userAuthenticationService;
    }

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
        this.timeProvider.Reset();
        this.userAuthenticationService.Reset();
    }

    protected virtual void DropDatabaseAfterTest()
    {
        if (this.settings.UseLocalDb || this.settings.TestRunMode == TestRunMode.DefaultRunModeOnEmptyDatabase || this.settings.TestsParallelize)
        {
            AssemblyInitializeAndCleanupBase.RunAction("Drop Database", this.databaseContext.Drop);
        }
    }

    protected virtual void ReattachDatabase()
    {
        switch (this.settings.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
            case TestRunMode.RestoreDatabaseUsingAttach:
                AssemblyInitializeAndCleanupBase.RunAction("Drop Database", this.databaseContext.Drop);
                AssemblyInitializeAndCleanupBase.RunAction("Restore Databases", this.databaseContext.AttachDatabase);
                break;
        }
    }

    protected virtual void CleanupTestEnvironment()
    {
    }
}
