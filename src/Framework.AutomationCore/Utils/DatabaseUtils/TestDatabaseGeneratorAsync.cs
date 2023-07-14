using Automation.Utils.DatabaseUtils.Interfaces;

namespace Automation.Utils.DatabaseUtils;

public abstract class TestDatabaseGeneratorAsync : BaseTestDatabaseGenerator, ITestDatabaseGeneratorAsync
{
    protected TestDatabaseGeneratorAsync(IDatabaseContext databaseContext, ConfigUtil configUtil)
        : base(databaseContext, configUtil)
    {
    }

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
