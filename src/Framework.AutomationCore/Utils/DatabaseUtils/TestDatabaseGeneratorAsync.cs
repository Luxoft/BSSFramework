using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;

namespace Automation.Utils.DatabaseUtils;

public abstract class AsyncTestDatabaseGenerator(
    IDatabaseContext databaseContext,
    IOptions<AutomationFrameworkSettings> settings)
    : BaseTestDatabaseGenerator(databaseContext, settings), ITestDatabaseGeneratorAsync
{
    public abstract Task GenerateDatabasesAsync();

    public async Task CheckAndCreateDetachedFilesAsync()
    {
        if (!new FileInfo(this.DatabaseContext.Main.CopyDataPath).Exists)
        {
            this.DatabaseContext.ReCreate();
            await this.GenerateDatabasesAsync();
            this.ExecuteInsertsForDatabases();
            await this.GenerateTestDataAsync();
            this.DatabaseContext.CopyDetachedFiles();
        }
    }

    public virtual Task CheckTestDatabaseAsync() => Task.CompletedTask;

    public abstract Task GenerateTestDataAsync();
}
