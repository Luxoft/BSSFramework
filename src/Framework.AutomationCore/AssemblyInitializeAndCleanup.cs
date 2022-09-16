using System;
using System.Diagnostics;

using Automation.Enums;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;

namespace Automation;

public class AssemblyInitializeAndCleanup
{
    private readonly ConfigUtil configUtil;

    private readonly TestDatabaseGenerator databaseGenerator;

    private readonly Action releaseServiceProviderAction;

    public AssemblyInitializeAndCleanup(ConfigUtil configUtil, TestDatabaseGenerator databaseGenerator, Action releaseServiceProviderAction)
    {
        this.configUtil = configUtil;
        this.databaseGenerator = databaseGenerator;
        this.releaseServiceProviderAction = releaseServiceProviderAction;
    }

    public static void RunAction(string name, Action action)
    {
        var watch = new Stopwatch();

        Console.WriteLine(name);

        watch.Start();
        action();
        watch.Stop();

        Console.WriteLine("Elapsed time - {0}", watch.Elapsed);
        Console.WriteLine();
        Console.WriteLine();
    }

    public void EnvironmentInitialize()
    {
        // if (!databaseUtil.DatabaseContext.MainDatabase.IsLocalDb)
        // {
        //     RunAction("Check Server Name in allowed list", databaseUtil.CheckServerAllowed);
        // }
        //Initialize(action);

        switch (this.configUtil.TestRunMode)
        {
            case TestRunMode.RestoreDatabaseUsingAttach:
                RunAction("Create LocalDB instance", this.databaseGenerator.CreateLocalDb);
                RunAction("Check Test Database", this.databaseGenerator.CheckTestDatabase);
                this.databaseGenerator.CheckAndCreateDetachedFiles();
                this.databaseGenerator.DatabaseContext.Drop();
                break;
            case TestRunMode.GenerateTestDataOnExistingDatabase:
                break;
            default:
                RunAction("Create LocalDB instance", this.databaseGenerator.CreateLocalDb);
                RunAction("Check Test Database", this.databaseGenerator.CheckTestDatabase);
                RunAction("Delete detached files", this.databaseGenerator.DeleteDetachedFiles);
                RunAction("Drop and Create Databases", this.databaseGenerator.DatabaseContext.ReCreate);
                RunAction("Generate All Databases", this.databaseGenerator.GenerateDatabases);
                RunAction("Insert Statements", this.databaseGenerator.ExecuteInsertsForDatabases);
                RunAction("Test Data Initialize", this.databaseGenerator.GenerateTestData);
                RunAction("Backup Databases", this.databaseGenerator.DatabaseContext.CopyDetachedFiles);
                RunAction("Drop Database", this.databaseGenerator.DatabaseContext.Drop);
                break;
        }
    }

    public void EnvironmentCleanup()
    {
        switch (this.configUtil.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
                RunAction("Check Test Database", this.databaseGenerator.CheckTestDatabase);
                RunAction("Drop Databases",this.databaseGenerator.DatabaseContext.Drop);
                RunAction("Delete detached files", this.databaseGenerator.DeleteDetachedFiles);
                RunAction("Delete LocalDB Instance", this.databaseGenerator.DeleteLocalDb);
                break;

            default:
                RunAction("Delete LocalDB Instance", this.databaseGenerator.DeleteLocalDb);
                break;
        }

        this.releaseServiceProviderAction();
    }
}
