using System.Diagnostics;

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

    protected virtual async Task CleanupAsync(AutomationFrameworkSettings settings, ITestDatabaseGenerator databaseGenerator)
    {
        switch (settings.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
                await databaseGenerator.CheckTestDatabaseAsync();
                databaseGenerator.DatabaseContext.Drop();
                databaseGenerator.DeleteDetachedFiles();
                databaseGenerator.DeleteLocalDb();
                break;

            default:
                databaseGenerator.DeleteLocalDb();
                break;
        }
    }


    protected virtual async Task InitializeAsync(AutomationFrameworkSettings settings, ITestDatabaseGenerator databaseGenerator)
    {
        switch (settings.TestRunMode)
        {
            case TestRunMode.RestoreDatabaseUsingAttach:
                databaseGenerator.CreateLocalDb();
                await databaseGenerator.CheckTestDatabaseAsync();
                await databaseGenerator.CheckAndCreateDetachedFilesAsync();
                databaseGenerator.DatabaseContext.Drop();
                break;
            case TestRunMode.GenerateTestDataOnExistingDatabase:
                break;
            default:
                databaseGenerator.CreateLocalDb();
                await databaseGenerator.CheckTestDatabaseAsync();
                databaseGenerator.DeleteDetachedFiles();
                databaseGenerator.DatabaseContext.ReCreate();
                await databaseGenerator.GenerateDatabasesAsync();
                databaseGenerator.ExecuteInsertsForDatabases();
                await databaseGenerator.GenerateTestDataAsync();
                databaseGenerator.DatabaseContext.CopyDetachedFiles();
                databaseGenerator.DatabaseContext.Drop();
                break;
        }
    }
}
