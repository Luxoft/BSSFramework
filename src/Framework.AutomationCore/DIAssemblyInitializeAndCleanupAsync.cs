using Automation.Enums;
using Automation.Interfaces;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

namespace Automation;

public class DiAssemblyInitializeAndCleanupAsync : IAssemblyInitializeAndCleanupAsync
{
    private readonly ConfigUtil configUtil;
    private readonly ITestDatabaseGeneratorAsync databaseGenerator;

    public DiAssemblyInitializeAndCleanupAsync(
        ConfigUtil configUtil,
        ITestDatabaseGeneratorAsync databaseGenerator)
    {
        this.configUtil = configUtil;
        this.databaseGenerator = databaseGenerator;
    }

    public async Task EnvironmentInitializeAsync()
    {
        switch (this.configUtil.TestRunMode)
        {
            case TestRunMode.RestoreDatabaseUsingAttach:
                this.databaseGenerator.CreateLocalDb();
                await this.databaseGenerator.CheckTestDatabaseAsync();
                await this.databaseGenerator.CheckAndCreateDetachedFilesAsync();
                this.databaseGenerator.DatabaseContext.Drop();
                break;
            case TestRunMode.GenerateTestDataOnExistingDatabase:
                break;
            default:
                this.databaseGenerator.CreateLocalDb();
                await this.databaseGenerator.CheckTestDatabaseAsync();
                this.databaseGenerator.DeleteDetachedFiles();
                this.databaseGenerator.DatabaseContext.ReCreate();
                await this.databaseGenerator.GenerateDatabasesAsync();
                this.databaseGenerator.ExecuteInsertsForDatabases();
                await this.databaseGenerator.GenerateTestDataAsync();
                this.databaseGenerator.DatabaseContext.CopyDetachedFiles();
                this.databaseGenerator.DatabaseContext.Drop();
                break;
        }
    }

    public async Task EnvironmentCleanupAsync()
    {
        switch (this.configUtil.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
                await this.databaseGenerator.CheckTestDatabaseAsync();
                this.databaseGenerator.DatabaseContext.Drop();
                this.databaseGenerator.DeleteDetachedFiles();
                this.databaseGenerator.DeleteLocalDb();
                break;

            default:
                this.databaseGenerator.DeleteLocalDb();
                break;
        }
    }
}
