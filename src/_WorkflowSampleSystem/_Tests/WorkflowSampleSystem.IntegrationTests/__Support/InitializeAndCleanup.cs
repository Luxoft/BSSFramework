using Microsoft.Data.SqlClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.IntegrationTests.__Support.Utils;
using WorkflowSampleSystem.IntegrationTests.__Support.Utils.Framework;

using Automation;
using Automation.Utils;

using Microsoft.Extensions.Configuration;

using WorkflowSampleSystem.IntegrationTests.Support.Utils;

namespace WorkflowSampleSystem.IntegrationTests.__Support
{
    [TestClass]
    public class InitializeAndCleanup
    {
        public static WorkflowSampleSystemDatabaseUtil DatabaseUtil { get; set; }

        [AssemblyInitialize]
        public static void EnvironmentInitialize(TestContext testContext)
        {
            AppSettings.Initialize(nameof(WorkflowSampleSystem) + "_");

            DatabaseUtil = new WorkflowSampleSystemDatabaseUtil();

            AssemblyInitializeAndCleanup.EnvironmentInitialize(DatabaseUtil);
        }

        [AssemblyCleanup]
        public static void EnvironmentCleanup()
        {
            AssemblyInitializeAndCleanup.EnvironmentCleanup(DatabaseUtil);
        }
    }
}
