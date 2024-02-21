using Automation.Utils.DatabaseUtils.Interfaces;

namespace Automation.Utils.DatabaseUtils;

public abstract class TestDatabaseGenerator : BaseTestDatabaseGenerator, ITestDatabaseGenerator
{
    protected TestDatabaseGenerator(IDatabaseContext databaseContext, ConfigUtil configUtil)
        : base(databaseContext, configUtil)
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
