using System;
using System.Diagnostics;

using Automation.Enums;
using Automation.Utils;

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

        public static void EnvironmentInitialize(BaseDatabaseUtil baseDatabaseUtil)
        {
            RunAction("Create new localdb instance if UseLocalDb = true", CoreDatabaseUtil.CreateLocalDb);

            //RunAction("Check Server Name in allowed list", baseDatabaseUtil.CheckServerAllowed);

            switch (ConfigUtil.TestRunMode)
            {
                case TestRunMode.RestoreDatabaseUsingAttach:
                    RunAction("Check Test Database", baseDatabaseUtil.CheckTestDatabase);
                    baseDatabaseUtil.CheckAndCreateDetachedFiles();
                    break;
                case TestRunMode.GenerateTestDataOnExistingDatabase:
                    break;
                default:
                    RunAction("Check Test Database", baseDatabaseUtil.CheckTestDatabase);
                    RunAction("Delete detached files", baseDatabaseUtil.DeleteDetachedFiles);
                    RunAction("Drop and Create Databases", baseDatabaseUtil.CreateDatabase);
                    RunAction("Generate All Databases", baseDatabaseUtil.GenerateDatabases);
                    RunAction("Insert Statements", baseDatabaseUtil.ExecuteInsertsForDatabases);
                    RunAction("Test Data Initialize", baseDatabaseUtil.GenerateTestData);
                    RunAction("Backup Databases", baseDatabaseUtil.CopyDetachedFiles);
                    break;
            }
        }

        public static void EnvironmentCleanup(BaseDatabaseUtil baseDatabaseUtil)
        {
            switch (ConfigUtil.TestRunMode)
            {
                case TestRunMode.DefaultRunModeOnEmptyDatabase:
                    RunAction("Check Test Database", baseDatabaseUtil.CheckTestDatabase);
                    RunAction("Drop All Databases", baseDatabaseUtil.DropAllDatabases);
                    baseDatabaseUtil.DeleteDetachedFiles();
                    break;

                default:
                    if (ConfigUtil.UseLocalDb) RunAction("Drop localdb", baseDatabaseUtil.DropAllDatabases);
                    break;
            }
        }
    }
}
