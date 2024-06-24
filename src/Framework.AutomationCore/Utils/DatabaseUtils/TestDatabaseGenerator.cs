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

    public virtual void GenerateDatabases()
    {
    }

    public void CheckAndCreateDetachedFiles()
    {
        if (!new FileInfo(this.DatabaseContext.Main.CopyDataPath).Exists)
        {
            this.DatabaseContext.ReCreate();
            this.GenerateDatabases();
            this.ExecuteInsertsForDatabases();
            this.GenerateTestData();
            this.DatabaseContext.CopyDetachedFiles();
        }
    }

    public virtual void CheckTestDatabase()
    {
    }

    public virtual void GenerateTestData()
    {
    }
}
