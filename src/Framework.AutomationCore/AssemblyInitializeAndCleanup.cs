using System;
using System.Diagnostics;
using Automation.Enums;
using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;

namespace Automation
{
    public static class AssemblyInitializeAndCleanup
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

        public static void EnvironmentInitialize(IDatabaseUtil databaseUtil)
        {
            // if (!databaseUtil.DatabaseContext.MainDatabase.IsLocalDb)
            // {
            //     RunAction("Check Server Name in allowed list", databaseUtil.CheckServerAllowed);
            // }

            switch (ConfigUtil.TestRunMode)
            {
                case TestRunMode.RestoreDatabaseUsingAttach:
                    RunAction("Check Test Database", databaseUtil.CheckTestDatabase);
                    databaseUtil.CheckAndCreateDetachedFiles();
                    databaseUtil.DropDatabase();
                    break;
                case TestRunMode.GenerateTestDataOnExistingDatabase:
                    break;
                default:
                    RunAction("Check Test Database", databaseUtil.CheckTestDatabase);
                    RunAction("Delete detached files", databaseUtil.DeleteDetachedFiles);
                    RunAction("Drop and Create Databases", databaseUtil.CreateDatabase);
                    RunAction("Generate All Databases", databaseUtil.GenerateDatabases);
                    RunAction("Insert Statements", databaseUtil.ExecuteInsertsForDatabases);
                    RunAction("Test Data Initialize", databaseUtil.GenerateTestData);
                    RunAction("Backup Databases", databaseUtil.CopyDetachedFiles);
                    RunAction("Drop Database", databaseUtil.DropDatabase);
                    break;
            }
        }

        public static void EnvironmentCleanup(IDatabaseUtil databaseUtil)
        {
            switch (ConfigUtil.TestRunMode)
            {
                case TestRunMode.DefaultRunModeOnEmptyDatabase:
                    RunAction("Check Test Database", databaseUtil.CheckTestDatabase);
                    RunAction("Drop Databases",databaseUtil.DropDatabase);
                    RunAction("Delete detached files", databaseUtil.DeleteDetachedFiles);
                    break;

                default:
                    RunAction("Delete LocalDB Instances", () => databaseUtil.DatabaseContext.Dispose());
                    break;
            }
        }
    }
}
