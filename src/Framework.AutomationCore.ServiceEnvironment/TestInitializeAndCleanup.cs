using Automation.Enums;
using Automation.Interfaces;
using Automation.ServiceEnvironment.Services;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

namespace Automation.ServiceEnvironment;

public class TestInitializeAndCleanup : ITestInitializeAndCleanup
{
    private readonly ConfigUtil configUtil;

    private readonly IDatabaseContext databaseContext;

    private readonly IIntegrationTestDateTimeService dateTimeService;

    private readonly IIntegrationTestUserAuthenticationService userAuthenticationService;

    public TestInitializeAndCleanup(
        ConfigUtil configUtil,
        IDatabaseContext databaseContext,
        IIntegrationTestDateTimeService dateTimeService,
        IIntegrationTestUserAuthenticationService userAuthenticationService)
    {
        this.configUtil = configUtil;
        this.databaseContext = databaseContext;
        this.dateTimeService = dateTimeService;
        this.userAuthenticationService = userAuthenticationService;
    }

    public virtual void Initialize()
    {
        this.ReattachDatabase();
        this.ResetServices();
    }

    public virtual void Cleanup()
    {
        this.DropDatabaseAfterTest();
        this.CleanupTestEnvironment();
    }

    protected virtual void ResetServices()
    {
        this.dateTimeService.Reset();
        this.userAuthenticationService.Reset();
    }

    protected virtual void DropDatabaseAfterTest()
    {
        if (this.configUtil.UseLocalDb || this.configUtil.TestRunMode == TestRunMode.DefaultRunModeOnEmptyDatabase || this.configUtil.TestsParallelize)
        {
            AssemblyInitializeAndCleanup.RunAction("Drop Database", this.databaseContext.Drop);
        }
    }

    protected virtual void ReattachDatabase()
    {
        switch (this.configUtil.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
            case TestRunMode.RestoreDatabaseUsingAttach:
                AssemblyInitializeAndCleanup.RunAction("Drop Database", this.databaseContext.Drop);
                AssemblyInitializeAndCleanup.RunAction("Restore Databases", this.databaseContext.AttachDatabase);
                break;
        }
    }

    protected virtual void CleanupTestEnvironment()
    {
    }
}
