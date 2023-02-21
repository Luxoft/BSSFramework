using System;
using System.Diagnostics;

using Automation.Enums;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Automation;

public class AssemblyInitializeAndCleanup
{
    private readonly Action<IServiceProvider> releaseServiceProviderAction;
    private readonly Func<IServiceProvider> getServiceProviderAction;

    public AssemblyInitializeAndCleanup(
        Func<IServiceProvider> getServiceProviderAction,
        Action<IServiceProvider> releaseServiceProviderAction)
    {
        this.getServiceProviderAction = getServiceProviderAction;
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
        var serviceProvider = getServiceProviderAction.Invoke();
        var configUtil = serviceProvider.GetRequiredService<ConfigUtil>();
        var databaseGenerator = serviceProvider.GetRequiredService<TestDatabaseGenerator>();

        // if (!configUtil.UseLocalDb)
        // {
        //     RunAction("Check Server Name in allowed list", databaseGenerator.CheckServerAllowed);
        // }

        switch (configUtil.TestRunMode)
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

        this.releaseServiceProviderAction(serviceProvider);
    }

    public void EnvironmentCleanup()
    {
        var serviceProvider = getServiceProviderAction.Invoke();
        var configUtil = serviceProvider.GetRequiredService<ConfigUtil>();
        var databaseGenerator = serviceProvider.GetRequiredService<TestDatabaseGenerator>();

        switch (configUtil.TestRunMode)
        {
            case TestRunMode.DefaultRunModeOnEmptyDatabase:
                RunAction("Check Test Database", databaseGenerator.CheckTestDatabase);
                RunAction("Drop Databases",databaseGenerator.DatabaseContext.Drop);
                RunAction("Delete detached files", databaseGenerator.DeleteDetachedFiles);
                RunAction("Delete LocalDB Instance", databaseGenerator.DeleteLocalDb);
                break;

            default:
                RunAction("Delete LocalDB Instance", databaseGenerator.DeleteLocalDb);
                break;
        }

        this.releaseServiceProviderAction(serviceProvider);
    }
}
