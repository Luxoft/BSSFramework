﻿using System.Diagnostics;

using Automation.Enums;
using Automation.Settings;
using Automation.Utils.DatabaseUtils;

namespace Automation;

public class AssemblyInitializeAndCleanupBase
{
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

    protected virtual void Cleanup(AutomationFrameworkSettings settings, ITestDatabaseGenerator databaseGenerator)
    {
        switch (settings.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
                RunAction("Check Test Database", databaseGenerator.CheckTestDatabase);
                RunAction("Drop Databases", databaseGenerator.DatabaseContext.Drop);
                RunAction("Delete detached files", databaseGenerator.DeleteDetachedFiles);
                RunAction("Delete LocalDB Instance", databaseGenerator.DeleteLocalDb);
                break;

            default:
                RunAction("Delete LocalDB Instance", databaseGenerator.DeleteLocalDb);
                break;
        }
    }

    protected virtual void Initialize(AutomationFrameworkSettings settings, ITestDatabaseGenerator databaseGenerator)
    {
        switch (settings.TestRunMode)
        {
            case TestRunMode.RestoreDatabaseUsingAttach:
                RunAction("Create LocalDB instance", databaseGenerator.CreateLocalDb);
                RunAction("Check Test Database", databaseGenerator.CheckTestDatabase);
                databaseGenerator.CheckAndCreateDetachedFiles();
                databaseGenerator.DatabaseContext.Drop();
                break;
            case TestRunMode.GenerateTestDataOnExistingDatabase:
                break;
            default:
                RunAction("Create LocalDB instance", databaseGenerator.CreateLocalDb);
                RunAction("Check Test Database", databaseGenerator.CheckTestDatabase);
                RunAction("Delete detached files", databaseGenerator.DeleteDetachedFiles);
                RunAction("Drop and Create Databases", databaseGenerator.DatabaseContext.ReCreate);
                RunAction("Generate All Databases", databaseGenerator.GenerateDatabases);
                RunAction("Insert Statements", databaseGenerator.ExecuteInsertsForDatabases);
                RunAction("Test Data Initialize", databaseGenerator.GenerateTestData);
                RunAction("Backup Databases", databaseGenerator.DatabaseContext.CopyDetachedFiles);
                RunAction("Drop Database", databaseGenerator.DatabaseContext.Drop);
                break;
        }
    }
}
