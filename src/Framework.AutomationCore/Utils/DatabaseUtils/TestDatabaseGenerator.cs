using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;

namespace Automation.Utils.DatabaseUtils;

public abstract class TestDatabaseGenerator : BaseTestDatabaseGenerator, ITestDatabaseGenerator
{
    protected TestDatabaseGenerator(
        IDatabaseContext databaseContext,
        IOptions<AutomationFrameworkSettings> settings)
        : base(databaseContext, settings)
    {
    }

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

    public abstract Task GenerateDatabasesAsync();

    public abstract Task GenerateTestDataAsync();

    public virtual Task CheckTestDatabaseAsync() => Task.CompletedTask;
}
